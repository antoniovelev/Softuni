namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Enum;
    using VaporStore.Dtos;

    public static class Serializer
    {
        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var genre = context.Genres
                .Where(g => genreNames.Contains(g.Name))
                .Select(g => new
                {
                    Id = g.Id,
                    Genre = g.Name,
                    Games = g.Games.Where(x => x.Purchases.Any()).Select(x => new
                    {
                        Id = x.Id,
                        Title = x.Name,
                        Developer = x.Developer.Name,
                        Tags = string.Join(", ", x.GameTags.Select(gt => gt.Tag.Name).ToList()),
                        Players = x.Purchases.Count
                    })
                    .OrderByDescending(x => x.Players)
                    .ThenBy(x => x.Id)
                    .ToList(),

                    TotalPlayers = g.Games.Sum(y => y.Purchases.Count)
                })
                .OrderByDescending(g => g.TotalPlayers)
                .ThenBy(g => g.Id)
                .ToList();

            var json = JsonConvert.SerializeObject(genre, Formatting.Indented);

            return json;
        }

        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string storeType)
        {
            var sb = new StringBuilder();
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var xmlSerializer = new XmlSerializer(typeof(List<ExportUserDto>),
                new XmlRootAttribute("Users"));

            var purchaseType = Enum.Parse<PurchaseType>(storeType);

            var users = context.Users
                .Select(u => new ExportUserDto
                {
                    Username = u.Username,
                    Purchases = u.Cards
                        .SelectMany(c => c.Purchases)
                        .Where(p => p.Type == purchaseType)
                        .Select(p => new PurchasesDto
                        {
                            Card = p.Card.Number,
                            Cvc = p.Card.Cvc,
                            Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Game = new GameDto
                            {
                                Genre = p.Game.Genre.Name,
                                Title = p.Game.Name,
                                Price = p.Game.Price
                            }
                        })
                    .OrderBy(p => p.Date)
                    .ToList(),

                    TotalSpentMoney = u.Cards.SelectMany(c => c.Purchases)
                    .Where(p => p.Type == purchaseType)
                    .Sum(p => p.Game.Price)
                })
                .Where(u => u.Purchases.Any())
                .OrderByDescending(u => u.TotalSpentMoney)
                .ThenBy(u => u.Username)
                .ToList();


            using (var writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, users, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}
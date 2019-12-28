namespace VaporStore.DataProcessor
{
	using System;
	using Data;
    using Newtonsoft.Json;
    using System.Linq;
    using VaporStore.Data.Models;
    using System.Text;
    using VaporStore.Dtos;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Globalization;
    using VaporStore.Data.Enum;
    using System.Xml.Serialization;
    using System.IO;

    public static class Deserializer
	{
		public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
            var sb = new StringBuilder();
            var games = new List<Game>();
            var gameDtos = JsonConvert.DeserializeObject<ImportGameDto[]>(jsonString);

            foreach (var gameDto in gameDtos)
            {
                if (!IsValid(gameDto) || gameDto.Tags.Count == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var game = new Game
                {
                    Name = gameDto.Name,
                    Price = gameDto.Price,
                    ReleaseDate = DateTime.ParseExact(gameDto.ReleaseDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                };

                var developer = GetDeveloper(context, gameDto.Developer);
                var genre = GetGenre(context, gameDto.Genre);

                game.Developer = developer;
                game.Genre = genre;

                foreach (var currentTag in gameDto.Tags)
                {
                    var tag = GetTag(context, currentTag);
                    game.GameTags.Add(new GameTag
                    {
                        Game = game,
                        Tag = tag
                    });
                }

                games.Add(game);
                sb.AppendLine($"Added {game.Name} ({game.Genre.Name}) with {game.GameTags.Count} tags");
            }
            context.Games.AddRange(games);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static Tag GetTag(VaporStoreDbContext context, string currentTag)
        {
            var tag = context.Tags.FirstOrDefault(t => t.Name == currentTag);

            if (tag == null)
            {
                tag = new Tag
                {
                    Name = currentTag
                };
                context.Tags.Add(tag);
                context.SaveChanges();
            }

            return tag;
        }

        private static Genre GetGenre(VaporStoreDbContext context, string gameDtoGenre)
        {
            var genre = context.Genres.FirstOrDefault(g => g.Name == gameDtoGenre);

            if (genre == null)
            {
                genre = new Genre
                {
                    Name = gameDtoGenre
                };

                context.Genres.Add(genre);
                context.SaveChanges();
            }

            return genre;
        }

        private static Developer GetDeveloper(VaporStoreDbContext context, string gameDtoDeveloper)
        {
            var developer = context.Developers.FirstOrDefault(d => d.Name == gameDtoDeveloper);

            if (developer == null)
            {
                developer = new Developer
                {
                    Name = gameDtoDeveloper
                };

                context.Developers.Add(developer);
                context.SaveChanges();
            }

            return developer;
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
            var userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(jsonString);
            var sb = new StringBuilder();
            var users = new List<User>();

            foreach (var userDto in userDtos)
            {
                if (!IsValid(userDto) || !userDto.Cards.All(IsValid))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }
                

                foreach (var cardDto in userDto.Cards)
                {
                    var cardType = Enum.TryParse<CardType>(cardDto.Type, out CardType result);

                    if (!cardType)
                    {
                        sb.AppendLine("Invalid Data");
                        break;
                    }
                }

                var user = new User
                {
                    FullName = userDto.FullName,
                    Username = userDto.Username,
                    Age = userDto.Age,
                    Email = userDto.Email
                };

                foreach (var card in userDto.Cards)
                {
                    user.Cards.Add(new Card
                    {
                        Number = card.Number,
                        Cvc = card.Cvc,
                        Type = Enum.Parse<CardType>(card.Type)
                    });
                }
                users.Add(user);
                sb.AppendLine($"Imported {user.Username} with {user.Cards.Count} cards");
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
		}

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
            var xmlSerializer = new XmlSerializer(typeof(List<ImportPurchasesDto>),
                new XmlRootAttribute("Purchases"));

            var purchaseDtos = new List<ImportPurchasesDto>();

            using (var reader = new StringReader(xmlString))
            {
                purchaseDtos = (List<ImportPurchasesDto>) xmlSerializer.Deserialize(reader);
            }

            var sb = new StringBuilder();
            var purchases = new List<Purchase>();

            foreach (var purchaseDto in purchaseDtos)
            {
                if (!IsValid(purchaseDto))
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var isValidEnum = Enum.TryParse<PurchaseType>(purchaseDto.Type, out PurchaseType purchaseType);

                if (!isValidEnum)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var game = context.Games.FirstOrDefault(g => g.Name == purchaseDto.Title);
                var card = context.Cards.FirstOrDefault(c => c.Number == purchaseDto.Card);

                if (game == null || card == null)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

                var purchase = new Purchase
                {
                    Type = purchaseType,
                    Date = DateTime.ParseExact(purchaseDto.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                    ProductKey = purchaseDto.Key,
                    Game = game,
                    Card = card
                };
                purchases.Add(purchase);
                sb.AppendLine($"Imported {purchase.Game.Name} for {purchase.Card.User.Username}");
            }
            context.Purchases.AddRange(purchases);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
		}


        private static bool IsValid(object entity)
        {
            var validationContext = new ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(entity, validationContext, validationResult, true);

            return isValid; 
        }
	}
}
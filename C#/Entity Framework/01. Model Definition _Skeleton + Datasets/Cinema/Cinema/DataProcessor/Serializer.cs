namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Cinema.DataProcessor.ExportDto;
    using Data;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportTopMovies(CinemaContext context, int rating)
        {
            var movies = context.Movies
                .Where(m => m.Rating >= rating && m.Projections.Any())
                .Select(m => new
                {
                    MovieName = m.Title,
                    Rating = m.Rating.ToString("f2"),
                    TotalIncomes = m.Projections.Sum(p => p.Tickets.Sum(t => t.Price)).ToString("f2"),
                    Customers = m.Projections.Select(p => p.Tickets
                    .Select(t => new
                    {
                        FirstName = t.Customer.FirstName,
                        LastName = t.Customer.LastName,
                        Balance = t.Customer.Balance.ToString("f2")
                    })
                    .OrderByDescending(t => decimal.Parse(t.Balance))
                    .ThenBy(t => t.FirstName)
                    .ThenBy(t => t.LastName)
                    .ToList()
                    )
                })
                .OrderByDescending(m => double.Parse(m.Rating))
                .ThenByDescending(m => decimal.Parse(m.TotalIncomes))
                .Take(10)
                .ToList();

            var json = JsonConvert.SerializeObject(movies, Formatting.Indented);
            return json;
        }

        public static string ExportTopCustomers(CinemaContext context, int age)
        {
            var customers = context.Customers
                .Where(c => c.Age >= age)
                .Select(c => new CustomerDto
                {
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    SpentMoney = $"{c.Tickets.Sum(t => t.Price):f2}",
                    SpentTime = GetSpentTime(c.Tickets.Select(t => t.Projection.Movie.Duration))
                })
                
                .OrderByDescending(c => decimal.Parse(c.SpentMoney))
                .Take(10)
                .ToList();

            var xml = new XmlSerializer(typeof(List<CustomerDto>),
                new XmlRootAttribute("Customers"));

            var nsp = new XmlSerializerNamespaces();
            nsp.Add("", "");

            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                xml.Serialize(writer, customers, nsp);
            }

            return sb.ToString().TrimEnd();
        }

        private static string GetSpentTime(IEnumerable<TimeSpan> times)
        {
            var time = new TimeSpan();
            foreach (var item in times)
            {
                time += item;
            }

            return time.ToString(@"hh\:mm\:ss");
        }
    }
}
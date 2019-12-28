namespace Cinema.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Cinema.Data.Models;
    using Cinema.Data.Models.Enums;
    using Cinema.DataProcessor.ImportDto;
    using Data;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";
        private const string SuccessfulImportMovie 
            = "Successfully imported {0} with genre {1} and rating {2}!";
        private const string SuccessfulImportHallSeat 
            = "Successfully imported {0}({1}) with {2} seats!";
        private const string SuccessfulImportProjection 
            = "Successfully imported projection {0} on {1}!";
        private const string SuccessfulImportCustomerTicket 
            = "Successfully imported customer {0} {1} with bought tickets: {2}!";

        public static string ImportMovies(CinemaContext context, string jsonString)
        {
            var json = JsonConvert.DeserializeObject<MovieDto[]>(jsonString);
            var sb = new StringBuilder();

            var movies = new List<Movie>();

            foreach (var dto in json)
            {
                var isGenreValid = Enum.TryParse(dto.Genre, out Genre genre);

                if (IsValid(dto) && isGenreValid)
                {
                    var movie = new Movie
                    {
                        Title = dto.Title,
                        Genre = genre,
                        Duration = TimeSpan.ParseExact(dto.Duration, "c", CultureInfo.InvariantCulture),
                        Rating = dto.Rating,
                        Director = dto.Director
                    };

                    if (movies.Contains(movie))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    movies.Add(movie);
                    sb.AppendLine($"Successfully imported {movie.Title} with genre {movie.Genre} and rating {movie.Rating:f2}!");
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
            }

            context.Movies.AddRange(movies);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object entity)
        {
            var validateContext = new ValidationContext(entity);
            var validationResult = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(entity, validateContext, validationResult, true);

            return isValid;
        }

        public static string ImportHallSeats(CinemaContext context, string jsonString)
        {
            var json = JsonConvert.DeserializeObject<HallDto[]>(jsonString);

            var sb = new StringBuilder();
            var halls = new List<Hall>();

            foreach (var dto in json)
            {

                if (IsValid(dto))
                {
                    var hall = new Hall
                    {
                        Name = dto.Name,
                        Is3D = dto.Is3D,
                        Is4Dx = dto.Is4Dx,
                    };

                    var seats = new List<Seat>();

                    for (int i = 0; i < dto.Seats; i++)
                    {
                        var seat = new Seat
                        {
                            Hall = hall
                        };

                        seats.Add(seat);
                    }

                    hall.Seats = seats;
                    halls.Add(hall);

                    if (hall.Is3D && hall.Is4Dx)
                    {
                        sb.AppendLine($"Successfully imported {hall.Name}(4Dx/3D) with {hall.Seats.Count} seats!");
                    }
                    else if (hall.Is3D)
                    {
                        sb.AppendLine($"Successfully imported {hall.Name}(3D) with {hall.Seats.Count} seats!");
                    }
                    else if (hall.Is4Dx)
                    {
                        sb.AppendLine($"Successfully imported {hall.Name}(4Dx) with {hall.Seats.Count} seats!");
                    }
                    else
                    {
                        sb.AppendLine($"Successfully imported {hall.Name}(Normal) with {hall.Seats.Count} seats!");
                    }
                   
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
            }

            context.Halls.AddRange(halls);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProjections(CinemaContext context, string xmlString)
        {
            var sb = new StringBuilder();
            var xml = new XmlSerializer(typeof(List<ProjectonDto>),
                new XmlRootAttribute("Projections"));

            var projectionsDtos = new List<ProjectonDto>();

            using (var reader = new StringReader(xmlString))
            {
                projectionsDtos = (List<ProjectonDto>)xml.Deserialize(reader);
            }

            var projections = new List<Projection>();

            foreach (var dto in projectionsDtos)
            {
                var movie = context.Movies.FirstOrDefault(m => m.Id == dto.MovieId);
                var hall = context.Halls.FirstOrDefault(h => h.Id == dto.HallId);

                if (IsValid(dto) && movie != null && hall != null)
                {
                    var projection = new Projection
                    {
                        MovieId = movie.Id,
                        HallId = hall.Id,
                        DateTime = DateTime.ParseExact(dto.DateTime, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
                    };

                    projections.Add(projection);
                    sb.AppendLine($"Successfully imported projection {movie.Title} on {projection.DateTime.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}!");
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
            }

            context.Projections.AddRange(projections);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCustomerTickets(CinemaContext context, string xmlString)
        {
            var xml = new XmlSerializer(typeof(List<CustomerDto>),
                new XmlRootAttribute("Customers"));

            var customerDtos = new List<CustomerDto>();
            var sb = new StringBuilder();

            using (var reader = new StringReader(xmlString))
            {
                customerDtos = (List<CustomerDto>)xml.Deserialize(reader);
            }

            var customers = new List<Customer>();

            foreach (var dto in customerDtos)
            {
                if (IsValid(dto))
                {
                    var customer = new Customer
                    {
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        Age = dto.Age,
                        Balance = dto.Balance
                    };

                    var tickets = new List<Ticket>();
                    foreach (var currentTicket in dto.Tickets)
                    {
                        if (!IsValid(currentTicket))
                        {
                            sb.AppendLine(ErrorMessage);
                            continue;
                            
                        }

                        var ticket = new Ticket
                        {
                            ProjectionId = currentTicket.ProjectionId,
                            Price = currentTicket.Price
                        };
                        tickets.Add(ticket);
                    }

                    customer.Tickets = tickets;
                    customers.Add(customer);
                    sb.AppendLine($"Successfully imported customer {customer.FirstName} {customer.LastName} with bought tickets: {customer.Tickets.Count}!");
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
    }
}
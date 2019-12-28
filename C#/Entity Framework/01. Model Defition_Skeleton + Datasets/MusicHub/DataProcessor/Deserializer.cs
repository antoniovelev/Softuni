namespace MusicHub.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using MusicHub.Data.Models;
    using MusicHub.Data.Models.Enums;
    using MusicHub.DataProcessor.ImportDtos;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data";

        private const string SuccessfullyImportedWriter 
            = "Imported {0}";
        private const string SuccessfullyImportedProducerWithPhone 
            = "Imported {0} with phone: {1} produces {2} albums";
        private const string SuccessfullyImportedProducerWithNoPhone
            = "Imported {0} with no phone number produces {1} albums";
        private const string SuccessfullyImportedSong 
            = "Imported {0} ({1} genre) with duration {2}";
        private const string SuccessfullyImportedPerformer
            = "Imported {0} ({1} songs)";

        public static string ImportWriters(MusicHubDbContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var json = JsonConvert.DeserializeObject<WritersDto[]>(jsonString);

            var writers = new List<Writer>();

            foreach (var dto in json)
            {
                if (IsValid(dto))
                {
                    var writer = new Writer
                    {
                        Name = dto.Name,
                        Pseudonym = dto.Pseudonym
                    };
                    writers.Add(writer);
                    sb.AppendLine($"Imported {writer.Name}");
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
            }

            context.Writers.AddRange(writers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducersAlbums(MusicHubDbContext context, string jsonString)
        {
            var producerDtos = JsonConvert.DeserializeObject<ProducerDto[]>(jsonString);
            var sb = new StringBuilder();

            var producers = new List<Producer>();

            foreach (var dto in producerDtos)
            {
                var albums = new List<Album>();
                bool haveError = false;

                if (IsValid(dto))
                {
                    foreach (var album in dto.Albums)
                    {
                        if (IsValid(album))
                        {
                            var currentAlbum = new Album
                            {
                                Name = album.Name,
                                ReleaseDate = DateTime.ParseExact(album.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                            };

                            albums.Add(currentAlbum);
                        }
                        else
                        {
                            sb.AppendLine(ErrorMessage);
                            haveError = true;
                        }
                    }
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (haveError)
                {
                    continue;
                }

                var producer = new Producer
                {
                    Name = dto.Name,
                    PhoneNumber = dto.PhoneNumber,
                    Pseudonym = dto.Pseudonym,
                    Albums = albums
                };

                if (string.IsNullOrEmpty(producer.PhoneNumber))
                {
                    sb.AppendLine($"Imported {producer.Name} with no phone number produces {producer.Albums.Count} albums");
                }
                else
                {
                    sb.AppendLine($"Imported {producer.Name} with phone: {producer.PhoneNumber} produces {producer.Albums.Count} albums");
                }

                producers.Add(producer);
            }

            context.Producers.AddRange(producers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongs(MusicHubDbContext context, string xmlString)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<SongDto>),
                new XmlRootAttribute("Songs"));

            var songDtos = new List<SongDto>();

            using (var reader = new StringReader(xmlString))
            {
                songDtos = (List<SongDto>)xmlSerializer.Deserialize(reader);
            }

            var sb = new StringBuilder();
            var songs = new List<Song>();

            foreach (var dto in songDtos)
            {
                var currentGenre = Enum.TryParse<Genre>(dto.Genre, out Genre genre);

                var album = context.Albums.FirstOrDefault(a => a.Id == dto.AlbumId);

                if (dto.WriterId > 23 || album == null || currentGenre == false)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (IsValid(dto))
                {

                    var song = new Song
                    {
                        Name = dto.Name,
                        Duration = TimeSpan.ParseExact(dto.Duration, "c", CultureInfo.InvariantCulture),
                        CreatedOn = DateTime.ParseExact(dto.CreatedOn, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        Genre = Enum.Parse<Genre>(dto.Genre),
                        AlbumId = dto.AlbumId,
                        WriterId = dto.WriterId,
                        Price = dto.Price
                    };

                    songs.Add(song);
                    sb.AppendLine($"Imported {song.Name} ({song.Genre} genre) with duration {song.Duration}");
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
            }

            context.Songs.AddRange(songs);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSongPerformers(MusicHubDbContext context, string xmlString)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<SongPerformerDto>),
                new XmlRootAttribute("Performers"));

            var dtos = new List<SongPerformerDto>();

            using (var reader = new StringReader(xmlString))
            {
                dtos = (List<SongPerformerDto>)xmlSerializer.Deserialize(reader);
            }

            var performers = new List<Performer>();
            var sb = new StringBuilder();

            foreach (var dto in dtos)
            {
                var performerSongs = new List<SongPerformer>();

                if (IsValid(dto))
                {
                    var performer = new Performer
                    {
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        Age = dto.Age,
                        NetWorth = dto.NetWorth,
                    };

                    bool haveError = false;

                    foreach (var song in dto.PerformerSongs)
                    {
                        var currentSong = context.Songs.FirstOrDefault(s => s.Id == song.SongId);

                        if (currentSong == null)
                        {
                            haveError = true;
                            continue;
                        }
                        
                        var songPerformer = new SongPerformer
                        {
                            Performer = performer,
                            Song = currentSong
                        };

                        performerSongs.Add(songPerformer);
                    }

                    if (haveError)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    performer.PerformerSongs = performerSongs;
                    performers.Add(performer);

                    sb.AppendLine($"Imported {performer.FirstName} ({performer.PerformerSongs.Count} songs)");
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
            }
            context.Performers.AddRange(performers);
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
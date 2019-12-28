namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var json = JsonConvert.DeserializeObject<DepartmentDto[]>(jsonString);
            var sb = new StringBuilder();

            var departments = new List<Department>();

            foreach (var dto in json)
            {
                if (IsValid(dto))
                {
                    var cells = new List<Cell>();
                    bool haveEror = false;

                    foreach (var cell in dto.Cells)
                    {
                        if (IsValid(cell))
                        {
                            var currentCell = new Cell
                            {
                                CellNumber = cell.CellNumber,
                                HasWindow = cell.HasWindow
                            };

                            cells.Add(currentCell);
                        }
                        else
                        {
                            haveEror = true;
                            break;
                        }
                    }

                    if (haveEror)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var department = new Department
                    {
                        Name = dto.Name,
                        Cells = cells
                    };
                    departments.Add(department);
                    sb.AppendLine($"Imported {department.Name} with {department.Cells.Count} cells");
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var json = JsonConvert.DeserializeObject<PrisonersDto[]>(jsonString);
            var sb = new StringBuilder();

            var prisoners = new List<Prisoner>();

            foreach (var dto in json)
            {
                if (IsValid(dto))
                {
                    var mails = new List<Mail>();
                    bool haveError = false;

                    foreach (var mail in dto.Mails)
                    {
                        if (IsValid(mail))
                        {
                            var currentMail = new Mail
                            {
                                Description = mail.Description,
                                Sender = mail.Sender,
                                Address = mail.Address
                            };

                            mails.Add(currentMail);
                        }
                        else
                        {
                            haveError = true;
                            break;
                        }
                    }

                    if (haveError)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var prisoner = new Prisoner
                    {
                        FullName = dto.FullName,
                        Nickname = dto.Nickname,
                        Age = dto.Age,
                        IncarcerationDate = DateTime.ParseExact(dto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ReleaseDate = GetDate(dto.ReleaseDate),
                        Bail = dto.Bail,
                        CellId = dto.CellId,
                        Mails = mails
                    };

                    prisoners.Add(prisoner);
                    sb.AppendLine($"Imported {prisoner.FullName} {prisoner.Age} years old");
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static DateTime GetDate(string releaseDate)
        {
            if (releaseDate == null)
            {
                return new DateTime();
            }
            return DateTime.ParseExact(releaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<OfficerDto>),
                new XmlRootAttribute("Officers"));

            var dtos = new List<OfficerDto>();

            using (var reader = new StringReader(xmlString))
            {
                dtos = (List<OfficerDto>)xmlSerializer.Deserialize(reader);
            }

            var sb = new StringBuilder();
            var officers = new List<Officer>();

            foreach (var dto in dtos)
            {
                var isPostionValid = Enum.TryParse(dto.Position, out Position position);
                var isWeaponVlid = Enum.TryParse(dto.Weapon, out Weapon weapon);

                var officerPrisoners = new List<OfficerPrisoner>();

                if (IsValid(dto) && isPostionValid && isWeaponVlid)
                {
                    var officer = new Officer
                    {
                        FullName = dto.FullName,
                        Salary = dto.Salary,
                        Position = position,
                        Weapon = weapon,
                        DepartmentId = dto.DepartmentId
                    };

                    foreach (var currentPrisoner in dto.Prisoners)
                    {
                        var prisoner = context.Prisoners.FirstOrDefault(p => p.Id == currentPrisoner.Id);

                        var currentOficcerPrisoner = new OfficerPrisoner
                        {
                            Officer = officer,
                            Prisoner = prisoner
                        };

                        officerPrisoners.Add(currentOficcerPrisoner);
                    }

                    officer.OfficerPrisoners = officerPrisoners;
                    officers.Add(officer);
                    sb.AppendLine($"Imported {officer.FullName} ({officer.OfficerPrisoners.Count} prisoners");
                }
                else
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
            }

            context.Officers.AddRange(officers);
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
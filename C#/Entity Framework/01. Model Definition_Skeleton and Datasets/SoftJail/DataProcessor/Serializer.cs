namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .Where(p => ids.Contains(p.Id))
                .Select(p => new
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers.Select(o => new
                    {
                        OfficerName = o.Officer.FullName,
                        Department = o.Officer.Department.Name
                    })
                    .OrderBy(o => o.OfficerName)
                    //.ThenBy(p => p.Department)
                    .ToList(),

                    TotalOfficerSalary = p.PrisonerOfficers.Sum(o => o.Officer.Salary).ToString("f2")
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToList();

            var json = JsonConvert.SerializeObject(prisoners, Formatting.Indented);
            return json;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var xmlSerializer = new XmlSerializer(typeof(List<PrisonerDto>),
                new XmlRootAttribute("Prisoners"));

            var tokens = prisonersNames.Split(",");

            var prisoners = context.Prisoners
                .Where(p => tokens.Contains(p.FullName))
                .Select(p => new PrisonerDto
                {
                    Id = p.Id,
                    Name = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EncryptedMessages = p.Mails.Select(m => new EncryptedMessagesDto
                    {
                        Description = ReveseDescription(m.Description)
                    })
                    .ToList()
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToList();

            var sb = new StringBuilder();
            var nsp = new XmlSerializerNamespaces();
            nsp.Add("", "");

            using (var writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, prisoners, nsp);
            }

            return sb.ToString().TrimEnd();
        }

        private static string ReveseDescription(string description)
        {
            var list = description.ToList();

            list.Reverse();

            return string.Join("", list);
        }
    }
}
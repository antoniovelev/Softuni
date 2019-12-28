using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace VaporStore.Dtos
{
    [XmlType("User")]
    public class ExportUserDto
    {
        [Required]
        [MinLength(3), MaxLength(20)]
        [XmlAttribute("username")]
        public string Username { get; set; }

        [Required]
        [XmlArray("Purchases")]
        public List<PurchasesDto> Purchases { get; set; }
        
        [Required]
        [XmlElement("TotalSpent")]
        public decimal TotalSpentMoney { get; set; }
    }
}

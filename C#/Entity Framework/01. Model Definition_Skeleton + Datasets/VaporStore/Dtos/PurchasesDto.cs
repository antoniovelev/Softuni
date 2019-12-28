using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace VaporStore.Dtos
{
    [XmlType("Purchase")]
    public class PurchasesDto
    {
        [Required]
        [RegularExpression("[0-9][0-9][0-9][0-9] [0-9][0-9][0-9][0-9] [0-9][0-9][0-9][0-9] [0-9][0-9][0-9][0-9]")]
        [XmlElement("Card")]
        public string Card { get; set; }

        [Required]
        [RegularExpression("[0-9]{3}")]
        [XmlElement("Cvc")]
        public string Cvc { get; set; }

        [Required]
        [XmlElement("Date")]
        public string Date { get; set; }

        [Required]
        [XmlElement("Game")]
        public GameDto Game { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace VaporStore.Dtos
{
    [XmlType("Game")]
    public class GameDto
    {
        [Required]
        [XmlAttribute("title")]
        public string Title { get; set; }

        [Required]
        [XmlElement("Genre")]
        public string Genre { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        [XmlElement("Price")]
        public decimal Price { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Serialization;

namespace Cinema.DataProcessor.ImportDto
{
    [XmlType("Ticket")]
    public class TicketDto
    {
        [XmlElement("ProjectionId")]
        [Required]
        public int ProjectionId { get; set; }

        [XmlElement("Price")]
        [Required]
        [Range(1, 111111111111111)]
        public decimal Price { get; set; }
    }
}

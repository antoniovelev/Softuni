using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VaporStore.Data.Models;

namespace VaporStore.Dtos
{
    public class ImportGameDto
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        [Range(0, int.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public string ReleaseDate { get; set; }

        [Required]
        public string Developer { get; set; }
        
        [Required]
        public string Genre { get; set; }
        
        public List<string> Tags { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SULS.Models
{
    public class Submission
    {
        public Submission()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }

        [Required]
        [MaxLength(800)]
        public string Code { get; set; }

        [Required]
        public int AchievedResult  { get; set; }

        [Required]
        public DateTime CreatedOn  { get; set; }

        public Problem Problem { get; set; }

        public User user { get; set; }
    }
}

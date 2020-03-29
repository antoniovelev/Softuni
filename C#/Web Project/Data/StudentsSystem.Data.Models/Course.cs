namespace StudentsSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using StudentsSystem.Data.Common.Models;

    public class Course : BaseDeletableModel<string>
    {
        public Course()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Homeworks = new HashSet<Homework>();
        }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime StartOn { get; set; }

        [Required]
        public DateTime EndOn { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        [MaxLength(300)]
        public string Description { get; set; }

        [Required]
        [Range(2.00, 6.00)]
        public double Grade { get; set; }

        public ApplicationUser User { get; set; }

        [Required]
        public string UserId { get; set; }

        public IEnumerable<Homework> Homeworks { get; set; }
    }
}

namespace StudentsSystem.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using StudentsSystem.Data.Common.Models;

    public class Homework : BaseDeletableModel<string>
    {
        public Homework()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Exercises = new HashSet<Exercise>();
        }

        [Required]
        [MaxLength(80)]
        public string Name { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsReady { get; set; }

        [Required]
        public string CourseId { get; set; }

        public Course Course { get; set; }

        public IEnumerable<Exercise> Exercises { get; set; }
    }
}

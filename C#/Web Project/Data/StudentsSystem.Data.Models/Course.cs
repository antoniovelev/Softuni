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

        public double? Grade { get; set; }

        [Required]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public IEnumerable<Homework> Homeworks { get; set; }

        public IEnumerable<CourseEvent> CourseEvents { get; set; }
    }
}

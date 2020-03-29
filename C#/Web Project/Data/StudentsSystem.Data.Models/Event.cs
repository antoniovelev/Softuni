namespace StudentsSystem.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using StudentsSystem.Data.Common.Models;

    public class Event : BaseDeletableModel<string>
    {
        public Event()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string CourseId { get; set; }

        public Course Course { get; set; }
    }
}

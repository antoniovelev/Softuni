namespace StudentsSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using StudentsSystem.Data.Common.Models;

    public class CourseEvent : BaseDeletableModel<string>
    {
        [Required]
        public string CourseId { get; set; }

        public Course Course { get; set; }

        [Required]
        public string EventId { get; set; }

        public Event Event { get; set; }
    }
}

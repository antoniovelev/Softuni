namespace StudentsSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CourseEvent
    {
        [Required]
        public string CourseId { get; set; }

        public Course Course { get; set; }

        [Required]
        public string EventId { get; set; }

        public Event Event { get; set; }
    }
}

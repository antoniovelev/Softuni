namespace StudentsSystem.Web.ViewModels.Event
{
    using System.ComponentModel.DataAnnotations;

    using StudentsSystem.Data.Models;

    using StudentsSystem.Services.Mapping;

    public class CreateInputModel : IMapTo<Event>
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name should be between 2 and 100 characters", MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public string CourseId { get; set; }
    }
}

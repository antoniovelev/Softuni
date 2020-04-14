namespace StudentsSystem.Web.ViewModels.Event
{
    using System.ComponentModel.DataAnnotations;

    using StudentsSystem.Data.Models;
    using StudentsSystem.Services.Mapping;

    public class EditInputModel : IMapFrom<Event>
    {
        public string Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} should be between {2} and {1} characters", MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public string Date { get; set; }

        public string CourseId { get; set; }
    }
}

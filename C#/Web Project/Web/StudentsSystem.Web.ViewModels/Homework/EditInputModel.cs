namespace StudentsSystem.Web.ViewModels.Homework
{
    using System.ComponentModel.DataAnnotations;

    using StudentsSystem.Data.Models;
    using StudentsSystem.Services.Mapping;

    public class EditInputModel : IMapFrom<Homework>
    {
        public string Id { get; set; }

        [Required]
        [StringLength(80, ErrorMessage = "The {0} should be between {2} and {1} characters", MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public string EndDate { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsReady { get; set; } = false;

        public string CourseId { get; set; }

        public string UserUserId { get; set; }
    }
}

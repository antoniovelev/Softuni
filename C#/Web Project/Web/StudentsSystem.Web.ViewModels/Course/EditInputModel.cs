namespace StudentsSystem.Web.ViewModels.Course
{
    using System.ComponentModel.DataAnnotations;

    using StudentsSystem.Data.Models;
    using StudentsSystem.Services.Mapping;

    public class EditInputModel : IMapFrom<Course>
    {
        public string Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} should be between {2} and {1} characters", MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public string StartOn { get; set; }

        [Required]
        public string EndOn { get; set; }

        [Required]
        [Range(0, 30)]
        public int Duration { get; set; }

        [Required]
        [MaxLength(300)]
        public string Description { get; set; }

        public string UserUserId { get; set; }
    }
}

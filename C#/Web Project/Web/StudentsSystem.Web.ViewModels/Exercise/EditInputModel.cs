namespace StudentsSystem.Web.ViewModels.Exercise
{
    using System.ComponentModel.DataAnnotations;

    using StudentsSystem.Data.Models;
    using StudentsSystem.Services.Mapping;

    public class EditInputModel : IMapFrom<Exercise>
    {
        public string Id { get; set; }

        [Required]
        [StringLength(80, ErrorMessage = "The {0} should be between {2} and {1} characters", MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [MaxLength(int.MaxValue)]
        public string Condition { get; set; }

        [Required]
        public bool IsReady { get; set; } = false;

        public string HomeworkId { get; set; }

        public string UserUserId { get; set; }
    }
}

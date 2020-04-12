namespace StudentsSystem.Web.ViewModels.Homework
{
    using System.ComponentModel.DataAnnotations;

    using StudentsSystem.Data.Models;
    using StudentsSystem.Services.Mapping;

    public class CreateInputModel : IMapTo<Homework>
    {
        [Required]
        [StringLength(80, ErrorMessage = "Name should be between 2 and 100 characters", MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        public string EndDate { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsReady { get; set; } = false;

        [Required]
        public string CourseId { get; set; }

        public string UserUserId { get; set; }
    }
}

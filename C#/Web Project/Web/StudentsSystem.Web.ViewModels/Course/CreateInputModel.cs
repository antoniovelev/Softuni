namespace StudentsSystem.Web.ViewModels.Course
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using StudentsSystem.Data.Models;
    using StudentsSystem.Services.Mapping;

    public class CreateInputModel : IMapTo<Course>
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name should be between 2 and 100 characters", MinimumLength = 2)]
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

        [Required]
        public string Grade { get; set; }
    }
}

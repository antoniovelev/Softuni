namespace StudentsSystem.Web.ViewModels.Course
{
    using System.ComponentModel.DataAnnotations;

    using StudentsSystem.Data.Models;
    using StudentsSystem.Services.Mapping;

    public class DetailsViewModel : IMapFrom<Course>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [DisplayFormat(DataFormatString = "dd-MM-yyyy")]
        public string StartOn { get; set; }

        [DataType(DataType.Date)]

        [DisplayFormat(DataFormatString = "dd-MM-yyyy")]
        public string EndOn { get; set; }

        public int Duration { get; set; }

        public string Description { get; set; }

        public string Grade { get; set; }
    }
}

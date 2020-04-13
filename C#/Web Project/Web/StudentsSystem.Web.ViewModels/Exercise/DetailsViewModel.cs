namespace StudentsSystem.Web.ViewModels.Exercise
{
    using StudentsSystem.Data.Models;
    using StudentsSystem.Services.Mapping;

    public class DetailsViewModel : IMapFrom<Exercise>
    {
        public string Name { get; set; }

        public string Condition { get; set; }
    }
}

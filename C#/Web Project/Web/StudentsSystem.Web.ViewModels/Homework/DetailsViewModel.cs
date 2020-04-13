namespace StudentsSystem.Web.ViewModels.Homework
{
    using StudentsSystem.Data.Models;
    using StudentsSystem.Services.Mapping;

    public class DetailsViewModel : IMapFrom<Homework>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string EndDate { get; set; }

        public string Description { get; set; }

        public bool IsReady { get; set; } = false;

    }
}

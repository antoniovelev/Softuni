namespace StudentsSystem.Web.ViewModels.Event
{
    using StudentsSystem.Data.Models;
    using StudentsSystem.Services.Mapping;

    public class DetailsViewModel : IMapFrom<Event>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Date { get; set; }
    }
}

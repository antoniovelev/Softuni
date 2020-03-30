namespace StudentsSystem.Web.ViewModels.Event
{
    using System.Collections.Generic;

    using StudentsSystem.Data.Models;
    using StudentsSystem.Services.Mapping;

    public class AllViewModel : IMapFrom<Event>
    {
        public IEnumerable<Event> Events { get; set; }
    }
}

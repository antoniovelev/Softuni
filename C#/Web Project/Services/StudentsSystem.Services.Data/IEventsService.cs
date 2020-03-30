namespace StudentsSystem.Services.Data
{
    using System.Collections.Generic;

    using StudentsSystem.Data.Models;

    public interface IEventsService
    {
        IEnumerable<Event> GetAllEvents();
    }
}

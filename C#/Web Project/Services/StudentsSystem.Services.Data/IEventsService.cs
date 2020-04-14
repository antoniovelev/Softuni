namespace StudentsSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using StudentsSystem.Data.Models;
    using StudentsSystem.Web.ViewModels.Event;

    public interface IEventsService
    {
        IEnumerable<Event> GetAllEvents();

        Task CreateEventAsync(string userId, CreateInputModel inputModel);

        T GetEventById<T>(string id);

        Event GetEventById(string id);

        Task DeleteByIdAsync(string id);

        Task UpdateAsync(EditInputModel inputModel);
    }
}

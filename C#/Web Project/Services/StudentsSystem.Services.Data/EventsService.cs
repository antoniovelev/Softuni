namespace StudentsSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using StudentsSystem.Data.Common.Repositories;
    using StudentsSystem.Data.Models;

    public class EventsService : IEventsService
    {
        private readonly IDeletableEntityRepository<Event> eventRepository;

        public EventsService(IDeletableEntityRepository<Event> eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        public IEnumerable<Event> GetAllEvents()
        {
            var allEvents = this.eventRepository.All().ToList();
            return allEvents;
        }
    }
}

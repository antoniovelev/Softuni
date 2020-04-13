namespace StudentsSystem.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using StudentsSystem.Data.Common.Repositories;
    using StudentsSystem.Data.Models;
    using StudentsSystem.Services.Mapping;
    using StudentsSystem.Web.ViewModels.Event;

    public class EventsService : IEventsService
    {
        private readonly IDeletableEntityRepository<Event> eventRepository;
        private readonly IDeletableEntityRepository<CourseEvent> mapTableRepository;

        public EventsService(
            IDeletableEntityRepository<Event> eventRepository,
            IDeletableEntityRepository<CourseEvent> mapTableRepository)
        {
            this.eventRepository = eventRepository;
            this.mapTableRepository = mapTableRepository;
        }

        public async Task CreateEventAsync(string userId, CreateInputModel inputModel)
        {
            var newEvent = new Event
            {
                Name = inputModel.Name,
                Date = DateTime.ParseExact(inputModel.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                CourseId = inputModel.CourseId,
                UserId = userId,
            };

            var mapping = new CourseEvent
            {
                CourseId = newEvent.CourseId,
                EventId = newEvent.Id,
            };

            await this.mapTableRepository.AddAsync(mapping);
            await this.eventRepository.AddAsync(newEvent);
            await this.eventRepository.SaveChangesAsync();
            await this.mapTableRepository.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(string id)
        {
            var currentEvent = this.eventRepository.All().Where(x => x.Id == id).FirstOrDefault();
            this.eventRepository.Delete(currentEvent);
            await this.eventRepository.SaveChangesAsync();
        }

        public IEnumerable<Event> GetAllEvents()
        {
            var allEvents = this.eventRepository.All().ToList();
            return allEvents;
        }

        public T GetEventById<T>(string id)
        {
            var currentEvent = this.eventRepository.All().Where(x => x.Id == id).To<T>().FirstOrDefault();

            return currentEvent;
        }
    }
}

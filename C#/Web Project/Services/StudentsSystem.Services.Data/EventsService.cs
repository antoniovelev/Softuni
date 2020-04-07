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

        public EventsService(IDeletableEntityRepository<Event> eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        public async Task CreateEventAsync(CreateInputModel inputModel)
        {
            var newEvent = new Event
            {
                Name = inputModel.Name,
                Date = DateTime.ParseExact(inputModel.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                CourseId = inputModel.CourseId,
            };

            await this.eventRepository.AddAsync(newEvent);
            await this.eventRepository.SaveChangesAsync();
        }

        public IEnumerable<Event> GetAllEvents()
        {
            var allEvents = this.eventRepository.All().ToList();
            return allEvents;
        }
    }
}

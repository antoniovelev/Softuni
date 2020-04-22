namespace StudentsSystem.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Moq;
    using StudentsSystem.Data;
    using StudentsSystem.Data.Models;
    using StudentsSystem.Data.Repositories;
    using StudentsSystem.Web.ViewModels.Event;
    using Xunit;

    public class EventsServicesTests
    {
        private Mock<IEventsService> mockService;
        private ApplicationDbContext context;
        private EventsService service;

        public EventsServicesTests()
        {
            this.context = BaseServiceTests.CreateDbContext();
            var repository = new EfDeletableEntityRepository<Event>(this.context);
            var mapTableRepository = new EfDeletableEntityRepository<CourseEvent>(this.context);
            this.service = new EventsService(repository, mapTableRepository);
            this.mockService = new Mock<IEventsService>();
        }

        [Fact]
        public void GetAllEventsCorrect()
        {
            var data = new List<Event>
            {
                new Event { Id = Guid.NewGuid().ToString(), Name = "test", },
                new Event { Id = Guid.NewGuid().ToString(), Name = "test", },
                new Event { Id = Guid.NewGuid().ToString(), Name = "test", },
            };

            this.context.Events.AddRange(data);
            this.context.SaveChanges();
            var result = this.service.GetAllEvents().Count();
            Assert.Equal(3, result);
        }

        [Fact]
        public async Task CreateEventCorrect()
        {
            var courseId = Guid.NewGuid().ToString();
            var studentId = Guid.NewGuid().ToString();

            var inputModel = new CreateInputModel
            {
                Name = "Test",
                Date = "12-03-2020",
                CourseId = courseId,
            };

            var secondModel = new CreateInputModel
            {
                Name = "Test1",
                Date = "12-03-2020",
                CourseId = courseId,
            };

            await this.service.CreateEventAsync(studentId, inputModel);
            await this.service.CreateEventAsync(studentId, secondModel);

            var count = this.service.GetAllEvents().Count();
            Assert.NotNull(inputModel);
            Assert.NotNull(secondModel);
            Assert.Equal(2, count);
        }

        [Fact]
        public async Task GetByIdCorrect()
        {
            var entity = new Event { Id = Guid.NewGuid().ToString(), Name = "test", };
            await this.context.AddAsync(entity);
            await this.context.SaveChangesAsync();

            var eventById = this.service.GetEventById(entity.Id);
            Assert.NotNull(eventById);
            Assert.Equal(eventById.Id, entity.Id);
            Assert.Equal(eventById.Name, entity.Name);
            Assert.Equal(eventById.Date, entity.Date);
        }

        [Fact]
        public async Task UpdateAsyncCorrect()
        {
            var entity = new Event { Id = Guid.NewGuid().ToString(), Name = "test", };
            await this.context.AddAsync(entity);
            await this.context.SaveChangesAsync();

            var editModel = new EditInputModel
            {
                Id = entity.Id,
                Name = "update",
                Date = "25-09-2019",
                CourseId = entity.CourseId,
            };

            await this.service.UpdateAsync(editModel);
            Assert.NotEqual("test", editModel.Name);
            Assert.Equal(editModel.Name, entity.Name);
        }

        [Fact]
        public async Task DeleteByIdCorrect()
        {
            var data = new List<Event>
            {
                new Event { Id = "1", Name = "test1", },
                new Event { Id = "2", Name = "test2", },
                new Event { Id = "3", Name = "test3", },
            };

            await this.context.Events.AddRangeAsync(data);
            await this.context.SaveChangesAsync();

            await this.service.DeleteByIdAsync("1");
            var result = this.service.GetAllEvents().Count();

            Assert.Equal(2, result);
        }
    }
}

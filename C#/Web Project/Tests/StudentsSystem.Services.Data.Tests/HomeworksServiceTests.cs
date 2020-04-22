namespace StudentsSystem.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;
    using StudentsSystem.Data;
    using StudentsSystem.Data.Common.Repositories;
    using StudentsSystem.Data.Models;
    using StudentsSystem.Data.Repositories;
    using StudentsSystem.Services.Data;
    using StudentsSystem.Web.ViewModels.Homework;
    using Xunit;

    public class HomeworksServiceTests
    {
        private Mock<IHomeworksService> mock;
        private HomeworksService service;
        private ApplicationDbContext context;

        public HomeworksServiceTests()
        {
            this.context = BaseServiceTests.CreateDbContext();
            var repository = new EfDeletableEntityRepository<Homework>(this.context);
            this.service = new HomeworksService(repository);
            this.mock = new Mock<IHomeworksService>();
        }

        [Fact]
        public void GetAllHomeworksCorrect()
        {
            var data = new List<Homework>
            {
                new Homework { Name = "tets1", Description = "test1" },
                new Homework { Name = "test2", Description = "test2" },
            };

            this.context.Homeworks.AddRange(data);
            this.context.SaveChanges();
            var result = this.service.GetAllHomeworks();
            Assert.Equal(data.Count, result.Count());
        }

        [Fact]
        public async Task CreateHomeworkCorrect()
        {
            var courseId = Guid.NewGuid().ToString();
            var studentId = Guid.NewGuid().ToString();

            var inputModel = new CreateInputModel
            {
                Name = "Tets",
                EndDate = "25-04-2020",
                Description = "test",
                IsReady = false,
                CourseId = courseId,
            };
            await this.service.CreateHomeworkAsync(studentId, inputModel);
            var actual = this.service.GetAllHomeworks().Count();
            Assert.NotNull(inputModel);
            Assert.Equal(1, actual);
        }

        [Fact]
        public void GetHomeworkCorrect()
        {
            var homework = new Homework
            {
                Id = "1",
                Name = "test",
                Description = "test",
            };
            this.context.Homeworks.Add(homework);
            this.context.SaveChanges();

            var homeworkById = this.service.GetById(homework.Id);
            Assert.NotNull(homeworkById);
            Assert.Equal(homework.Name, homeworkById.Name);
        }

        [Fact]
        public void GetHomeworkWithMapperCorrect()
        {
            var homeworkId = Guid.NewGuid().ToString();
            var homework = new Homework { Id = homeworkId, Name = "tets1", Description = "test1" };
            this.context.Homeworks.Add(homework);
            this.context.SaveChanges();

            var repo = new Mock<IDeletableEntityRepository<Homework>>();
            var homeworkService = new HomeworksService(repo.Object);

            var viewModel = this.mock.Setup(x => x.GetHomeworkById<DetailsViewModel>(homeworkId));
            Assert.NotNull(homework);
        }

        [Fact]
        public async Task DeleteCorrect()
        {
            var data = new List<Homework>
            {
                new Homework { Name = "tets1", Description = "test1" },
                new Homework { Name = "test2", Description = "test2" },
                new Homework { Name = "test3", Description = "test" },
            };
            this.context.Homeworks.AddRange(data);
            this.context.SaveChanges();

            var homework = this.service.GetAllHomeworks().Where(x => x.Name == "test2").FirstOrDefault();
            await this.service.DeleteByIdAsync(homework.Id);

            var result = this.service.GetAllHomeworks().Count();
            Assert.Equal(2, result);
        }

        [Fact]
        public async Task UpdateAsyncCorrect()
        {
            var homeworkId = Guid.NewGuid().ToString();
            var homework = new Homework
            {
                Id = homeworkId, Name = "tets1", Description = "test1",
            };

            this.context.Homeworks.Add(homework);
            await this.context.SaveChangesAsync();

            var editModel = new EditInputModel
            {
                Id = homeworkId,
                Name = "Update",
                EndDate = "24-03-2020",
                Description = "update description",
            };

            await this.service.UpdateAsync(editModel);
            var updateHomework = this.service.GetById(editModel.Id);

            Assert.NotEqual("tets1", updateHomework.Name);
            Assert.NotEqual("test1", updateHomework.Description);
            Assert.Equal(updateHomework.Name, homework.Name);
            Assert.Equal(updateHomework.Id, homework.Id);
        }
    }
}

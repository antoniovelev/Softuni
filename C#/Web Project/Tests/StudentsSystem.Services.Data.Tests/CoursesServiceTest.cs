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
    using StudentsSystem.Web.ViewModels.Course;
    using Xunit;

    public class CoursesServiceTest
    {
        private Mock<ICoursesService> mockService;
        private ApplicationDbContext context;
        private CoursesService service;

        public CoursesServiceTest()
        {
            this.context = BaseServiceTests.CreateDbContext();
            var repository = new EfDeletableEntityRepository<Course>(this.context);
            this.service = new CoursesService(repository);
            this.mockService = new Mock<ICoursesService>();
        }

        [Fact]
        public void GetAllCoursesCorrect()
        {
            this.mockService.Setup(x => x.GetAllCourses(null, 0)).Returns(new List<Course>
            {
                new Course { Name = "C# web", Description = "none" },
                new Course { Name = "EF", Description = "hard" },
            });

            Assert.Equal(2, this.mockService.Object.GetAllCourses().Count());
        }

        [Fact]
        public void CreateCourseCorrect()
        {
            var inputModel = new CreateInputModel
            {
                Name = "C# web",
                Description = "hard course",
            };

            var course = this.mockService.Setup(x => x.CreateAsync(Guid.NewGuid().ToString(), inputModel));
            Assert.NotNull(course);
        }

        [Fact]
        public void GetCourseByIdCorrect()
        {
            var courseId = Guid.NewGuid().ToString();
            var course = this.mockService.Setup(x => x.GetCourseById<DetailsViewModel>(courseId));
            Assert.NotNull(course);
        }

        [Fact]
        public void GetCourseCountCorrect()
        {
            this.mockService.Setup(x => x.GetAllCourses(null, 0)).Returns(new List<Course>
            {
                new Course { Name = "C# EF", Description = "Learning how to work with EF" },
                new Course { Name = "C# web", Description = "Hard course" },
                new Course { Name = "C# OPP", Description = "none" },
            });

            Assert.Equal(3, this.mockService.Object.GetAllCourses().Count());
        }

        [Fact]
        public void GetByIdCorrect()
        {
            var course = this.mockService.Setup(x => x.GetById(Guid.NewGuid().ToString()));
            Assert.NotNull(course);
        }

        [Fact]
        public async Task DeleteByIdCorrect()
        {
            var firstCourseId = Guid.NewGuid().ToString();
            var data = new List<Course>
            {
                new Course { Id = firstCourseId, Name = "C# EF", Description = "Learning how to work with EF" },
                new Course { Id = Guid.NewGuid().ToString(), Name = "Test2", Description = "Learning how to work with EF" },
                new Course { Id = Guid.NewGuid().ToString(), Name = "Test3", Description = "Learning how to work with EF" },
            };

            this.context.AddRange(data);
            this.context.SaveChanges();
            await this.service.DeleteByIdAsync(firstCourseId);
            Assert.Equal(2, this.service.GetAllCourses().Count());
        }

        [Fact]
        public async Task UpdateCourseNameCorrect()
        {
            var studentId = Guid.NewGuid().ToString();
            var courseId = Guid.NewGuid().ToString();
            var course = new Course
            {
                Id = courseId,
                Name = "C# web",
                Description = "hard course",
            };

            this.context.Add(course);
            await this.context.SaveChangesAsync();

            var editModel = new EditInputModel
            {
                Id = courseId,
                Name = "C# test",
                StartOn = "12-01-2020",
                EndOn = "25-05-2020",
                Duration = 4,
                Description = "none",
            };

            await this.service.UpdateAsync(editModel);
            var up = this.service.GetById(course.Id);
            Assert.NotEqual("C# web", up.Name);
        }
    }
}

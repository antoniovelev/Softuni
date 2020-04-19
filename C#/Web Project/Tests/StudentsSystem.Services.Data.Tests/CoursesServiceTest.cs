namespace StudentsSystem.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;
    using StudentsSystem.Data.Models;
    using StudentsSystem.Web.ViewModels.Course;
    using Xunit;

    public class CoursesServiceTest
    {
        private Mock<ICoursesService> mockService;

        public CoursesServiceTest()
        {
             this.mockService = new Mock<ICoursesService>();
        }

        [Fact]
        public void GetAllCoursesCorect()
        {
            this.mockService.Setup(x => x.GetAllCourses(null, 0)).Returns(new List<Course>
            {
                new Course { Name = "C# web", Description = "none" },
                new Course { Name = "EF", Description = "hard" },
            });

            Assert.Equal(2, this.mockService.Object.GetAllCourses().Count());
        }

        [Fact]
        public void CreateCourseCorect()
        {
            var inputModel = new CreateInputModel
            {
                Name = "C# web",
                Description = "hard course",
            };

            var course = this.mockService.Setup(x => x.CreateAsync(Guid.NewGuid().ToString(), inputModel));
            Assert.NotNull(course);
        }
    }
}

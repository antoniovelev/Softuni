namespace StudentsSystem.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsSystem.Data.Common.Repositories;
    using StudentsSystem.Data.Models;
    using StudentsSystem.Web.ViewModels.Course;

    public class CoursesService : ICoursesService
    {
        private readonly IDeletableEntityRepository<Course> courseRepository;

        public CoursesService(IDeletableEntityRepository<Course> courseRepository)
        {
            this.courseRepository = courseRepository;
        }

        public async Task CreateAsync(string userId, CreateInputModel inputModel)
        {
            var course = new Course
            {
                Name = inputModel.Name,
                StartOn = DateTime.ParseExact(inputModel.StartOn, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                EndOn = DateTime.ParseExact(inputModel.EndOn, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                Duration = inputModel.Duration,
                Description = inputModel.Description,
                Grade = double.Parse(inputModel.Grade),
                UserId = userId,
            };

            await this.courseRepository.AddAsync(course);
            await this.courseRepository.SaveChangesAsync();
        }

        public IEnumerable<Course> GetAllCourses()
        {
            var allCourses = this.courseRepository.All().ToList();
            return allCourses;
        }
    }
}

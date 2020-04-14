namespace StudentsSystem.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsSystem.Data.Common.Repositories;
    using StudentsSystem.Data.Models;
    using StudentsSystem.Services.Mapping;
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
                UserId = userId,
            };

            await this.courseRepository.AddAsync(course);
            await this.courseRepository.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(string id)
        {
            var course = this.courseRepository.All().Where(x => x.Id == id).FirstOrDefault();
            this.courseRepository.Delete(course);
            await this.courseRepository.SaveChangesAsync();
        }

        public IEnumerable<Course> GetAllCourses()
        {
            var allCourses = this.courseRepository.All().ToList();
            return allCourses;
        }

        public Course GetById(string id)
        {
            var course = this.courseRepository.All().FirstOrDefault(x => x.Id == id);
            return course;
        }

        public T GetCourseById<T>(string id)
        {
            var course = this.courseRepository.All().Where(x => x.Id == id).To<T>().FirstOrDefault();

            return course;
        }

        public async Task UpdateAsync(EditInputModel inputModel)
        {
            var course = this.courseRepository.All().FirstOrDefault(x => x.Id == inputModel.Id);

            course.Name = inputModel.Name;
            course.StartOn = DateTime.ParseExact(inputModel.StartOn, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            course.EndOn = DateTime.ParseExact(inputModel.EndOn, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            course.Duration = inputModel.Duration;
            course.Description = inputModel.Description;
            course.UserId = inputModel.UserUserId;

            this.courseRepository.Update(course);
            await this.courseRepository.SaveChangesAsync();
        }
    }
}

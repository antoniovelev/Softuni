namespace StudentsSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using StudentsSystem.Data.Common.Repositories;
    using StudentsSystem.Data.Models;

    public class CoursesService : ICoursesService
    {
        private readonly IDeletableEntityRepository<Course> courseRepository;

        public CoursesService(IDeletableEntityRepository<Course> courseRepository)
        {
            this.courseRepository = courseRepository;
        }

        public IEnumerable<Course> GetAllCourses()
        {
            var allCourses = this.courseRepository.All().ToList();
            return allCourses;
        }
    }
}

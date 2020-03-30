namespace StudentsSystem.Services.Data
{
    using System.Collections.Generic;

    using StudentsSystem.Data.Models;

    public interface ICoursesService
    {
        IEnumerable<Course> GetAllCourses();
    }
}

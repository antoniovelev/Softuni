namespace StudentsSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using StudentsSystem.Data.Models;
    using StudentsSystem.Web.ViewModels.Course;

    public interface ICoursesService
    {
        IEnumerable<Course> GetAllCourses();

        Task CreateAsync(string userId, CreateInputModel inputModel);

        T GetCourseById<T>(string id);

        Course GetById(string id);

        Task DeleteByIdAsync(string id);

        Task UpdateAsync(EditInputModel inputModel);
    }
}

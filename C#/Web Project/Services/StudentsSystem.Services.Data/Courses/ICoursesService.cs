namespace StudentsSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using StudentsSystem.Data.Models;
    using StudentsSystem.Web.ViewModels.Course;

    public interface ICoursesService
    {
        IEnumerable<Course> GetAllCourses(string userId, int? take = null, int skip = 0);

        IEnumerable<Course> GetAllOldCourses(string userId, int? take = null, int skip = 0);

        Task CreateAsync(string userId, CreateInputModel inputModel);

        T GetCourseById<T>(string id);

        Course GetById(string id);

        Task DeleteByIdAsync(string id);

        int GetCount(string userId); 

        int GetOldCoursesCount(string userId); 

        Task UpdateAsync(EditInputModel inputModel);
    }
}

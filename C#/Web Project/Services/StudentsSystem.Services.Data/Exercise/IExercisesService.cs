namespace StudentsSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using StudentsSystem.Data.Models;
    using StudentsSystem.Web.ViewModels.Exercise;

    public interface IExercisesService
    {
        IEnumerable<Exercise> GetAllExercise();

        Task CreateExerciseAsync(string userId, CreateInputModel inputModel);

        T GetExerciseById<T>(string id);

        Exercise GetById(string id);

        Task DeleteByIdAsync(string id);

        Task UpdateAsync(EditInputModel inputModel);
    }
}

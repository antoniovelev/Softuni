namespace StudentsSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using StudentsSystem.Data.Common.Repositories;
    using StudentsSystem.Data.Models;
    using StudentsSystem.Services.Mapping;
    using StudentsSystem.Web.ViewModels.Exercise;

    public class ExercisesService : IExercisesService
    {
        private readonly IDeletableEntityRepository<Exercise> exerciseRepository;

        public ExercisesService(IDeletableEntityRepository<Exercise> exerciseRepository)
        {
            this.exerciseRepository = exerciseRepository;
        }

        public async Task CreateExerciseAsync(string userId, CreateInputModel inputModel)
        {
            var exercise = new Exercise
            {
                Name = inputModel.Name,
                Condition = inputModel.Condition,
                IsReady = inputModel.IsReady,
                HomeworkId = inputModel.HomeworkId,
                UserId = userId,
            };

            await this.exerciseRepository.AddAsync(exercise);
            await this.exerciseRepository.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(string id)
        {
            var exercise = this.exerciseRepository.All().FirstOrDefault(x => x.Id == id);
            this.exerciseRepository.Delete(exercise);
            await this.exerciseRepository.SaveChangesAsync();
        }

        public IEnumerable<Exercise> GetAllExercise()
        {
            var exercises = this.exerciseRepository.All().ToList();
            return exercises;
        }

        public Exercise GetById(string id)
        {
            var exercise = this.exerciseRepository.All().FirstOrDefault(x => x.Id == id);

            return exercise;
        }

        public T GetExerciseById<T>(string id)
        {
            var exercise = this.exerciseRepository.All().Where(x => x.Id == id).To<T>().FirstOrDefault();

            return exercise;
        }

        public async Task UpdateAsync(EditInputModel inputModel)
        {
            var exercise = this.exerciseRepository.All().Where(x => x.Id == inputModel.Id).FirstOrDefault();

            exercise.Name = inputModel.Name;
            exercise.Condition = inputModel.Condition;
            exercise.HomeworkId = inputModel.HomeworkId;

            this.exerciseRepository.Update(exercise);
            await this.exerciseRepository.SaveChangesAsync();
        }
    }
}

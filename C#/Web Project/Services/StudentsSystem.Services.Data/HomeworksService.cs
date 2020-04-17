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
    using StudentsSystem.Web.ViewModels.Homework;

    public class HomeworksService : IHomeworksService
    {
        private readonly IDeletableEntityRepository<Homework> homeworkRepository;

        public HomeworksService(IDeletableEntityRepository<Homework> homeworkRepository)
        {
            this.homeworkRepository = homeworkRepository;
        }

        public async Task CreateHomeworkAsync(string userId, CreateInputModel inputModel)
        {
            var homework = new Homework
            {
                Name = inputModel.Name,
                EndDate = DateTime.ParseExact(inputModel.EndDate, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                Description = inputModel.Description,
                IsReady = inputModel.IsReady,
                CourseId = inputModel.CourseId,
                UserId = userId,
            };

            await this.homeworkRepository.AddAsync(homework);
            await this.homeworkRepository.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(string id)
        {
            var homework = this.homeworkRepository.All().Where(x => x.Id == id).FirstOrDefault();
            this.homeworkRepository.Delete(homework);
            await this.homeworkRepository.SaveChangesAsync();
        }

        public IEnumerable<Homework> GetAllHomeworks()
        {
            var homeworks = this.homeworkRepository.All().ToList();

            return homeworks;
        }

        public Homework GetById(string id)
        {
            var homework = this.homeworkRepository.All().FirstOrDefault(x => x.Id == id);
            return homework;
        }

        public T GetHomeworkById<T>(string id)
        {
            var homework = this.homeworkRepository.All().Where(x => x.Id == id).To<T>().FirstOrDefault();

            return homework;
        }

        public async Task UpdateAsync(EditInputModel inputModel)
        {
            var homework = this.homeworkRepository.All().FirstOrDefault(x => x.Id == inputModel.Id);
            homework.Name = inputModel.Name;
            homework.EndDate = DateTime.ParseExact(inputModel.EndDate, "dd-mm-yyyy", CultureInfo.InvariantCulture);
            homework.Description = inputModel.Description;
            homework.CourseId = inputModel.CourseId;

            this.homeworkRepository.Update(homework);
            await this.homeworkRepository.SaveChangesAsync();
        }
    }
}

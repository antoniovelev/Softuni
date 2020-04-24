﻿namespace StudentsSystem.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using StudentsSystem.Data.Models;
    using StudentsSystem.Web.ViewModels.Homework;

    public interface IHomeworksService
    {
        IEnumerable<Homework> GetAllHomeworks();

        Task CreateHomeworkAsync(string userId, CreateInputModel inputModel);

        T GetHomeworkById<T>(string id);

        Homework GetById(string id);

        Task DeleteByIdAsync(string id);

        Task UpdateAsync(EditInputModel inputModel);

        Task SetFinishedAsync(Homework homework);

        Task SetNotFinishedAsync(Homework homework);
    }
}

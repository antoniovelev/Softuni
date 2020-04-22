namespace StudentsSystem.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Moq;
    using StudentsSystem.Data;
    using StudentsSystem.Data.Models;
    using StudentsSystem.Data.Repositories;
    using StudentsSystem.Web.ViewModels.Exercise;
    using Xunit;

    public class ExercisesServicesTests
    {
        private Mock<IExercisesService> mock;
        private ExercisesService service;
        private ApplicationDbContext context;

        public ExercisesServicesTests()
        {
            this.context = BaseServiceTests.CreateDbContext();
            var repository = new EfDeletableEntityRepository<Exercise>(this.context);
            this.service = new ExercisesService(repository);
            this.mock = new Mock<IExercisesService>();
        }

        [Fact]
        public async Task GetAllExercisesCorrect()
        {
            var data = new List<Exercise>
            {
                new Exercise { Name = "test", Condition = "test", },
                new Exercise { Name = "test1", Condition = "test1", },
                new Exercise { Name = "test2", Condition = "test2", },
            };

            await this.context.Exercises.AddRangeAsync(data);
            await this.context.SaveChangesAsync();

            var result = this.service.GetAllExercise().Count();
            Assert.Equal(3, result);
        }

        [Fact]
        public async Task CreateAsyncCorrecr()
        {
            var model = new CreateInputModel
            {
                Name = "test",
                Condition = "test",
            };

            var secondModel = new CreateInputModel
            {
                Name = "test1",
                Condition = "test1",
            };

            await this.service.CreateExerciseAsync(Guid.NewGuid().ToString(), model);
            await this.service.CreateExerciseAsync(Guid.NewGuid().ToString(), secondModel);
            var result = this.service.GetAllExercise().Count();
            Assert.Equal(2, result);
        }

        [Fact]
        public async Task GetByIdCorrect()
        {
            var data = new List<Exercise>
            {
                new Exercise { Id = "1", Name = "test", Condition = "test", },
                new Exercise { Id = "2", Name = "test1", Condition = "test1", },
                new Exercise { Id = "3", Name = "test2", Condition = "test2", },
            };

            await this.context.Exercises.AddRangeAsync(data);
            await this.context.SaveChangesAsync();

            var exercise = this.service.GetById("1");
            Assert.Equal("test", exercise.Name);
            Assert.Equal("test", exercise.Condition);
        }

        [Fact]
        public async Task UpdateByIdAsyncCorrect()
        {
            var exercise = new Exercise { Id = Guid.NewGuid().ToString(), Name = "test", Condition = "test", };
            await this.context.Exercises.AddAsync(exercise);
            await this.context.SaveChangesAsync();

            var editModel = new EditInputModel
            {
                Id = exercise.Id,
                Name = "Update",
                Condition = "update",
            };
            await this.service.UpdateAsync(editModel);
            Assert.NotEqual("test", editModel.Name);
            Assert.NotEqual("test", editModel.Condition);
            Assert.Equal(editModel.Name, exercise.Name);
            Assert.Equal(editModel.Condition, exercise.Condition);
        }

        [Fact]
        public async Task DeleteByIdAsync()
        {
            var data = new List<Exercise>
            {
                new Exercise { Id = "1", Name = "test", Condition = "test", },
                new Exercise { Id = "2", Name = "test1", Condition = "test1", },
                new Exercise { Id = "3", Name = "test2", Condition = "test2", },
                new Exercise { Id = "4", Name = "test4", Condition = "test4", },
            };

            await this.context.Exercises.AddRangeAsync(data);
            await this.context.SaveChangesAsync();

            await this.service.DeleteByIdAsync("1");
            await this.service.DeleteByIdAsync("2");

            var result = this.service.GetAllExercise().Count();
            Assert.Equal(2, result);
        }
    }
}

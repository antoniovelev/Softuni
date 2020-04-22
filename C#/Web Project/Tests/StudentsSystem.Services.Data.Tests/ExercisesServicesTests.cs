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

            await this.context.AddRangeAsync(data);
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
    }
}

namespace StudentsSystem.Services.Data.Tests
{
    using System;

    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using StudentsSystem.Data;
    using StudentsSystem.Data.Common.Repositories;
    using StudentsSystem.Data.Repositories;

    public abstract class BaseServiceTests
    {
        public static ApplicationDbContext CreateDbContext()
        {
            var option = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;

            var context = new ApplicationDbContext(option);
            return context;
        }
    }
}

namespace StudentsSystem.Web.ViewModels.Exercise
{
    using System.Collections.Generic;

    using StudentsSystem.Data.Models;
    using StudentsSystem.Services.Mapping;

    public class AllViewModel : IMapFrom<Exercise>
    {
        public IEnumerable<Exercise> Exercises { get; set; }
    }
}

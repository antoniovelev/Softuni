namespace StudentsSystem.Web.ViewModels.Course
{
    using System.Collections.Generic;

    using StudentsSystem.Data.Models;
    using StudentsSystem.Services.Mapping;

    public class AllViewModel
    {
        public IEnumerable<Course> Courses { get; set; }
    }
}

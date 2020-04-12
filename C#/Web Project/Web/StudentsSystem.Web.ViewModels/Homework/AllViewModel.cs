namespace StudentsSystem.Web.ViewModels.Homework
{
    using System.Collections.Generic;

    using StudentsSystem.Data.Models;
    using StudentsSystem.Services.Mapping;

    public class AllViewModel : IMapFrom<Homework>
    {
        public IEnumerable<Homework> Homeworks { get; set; }
    }
}

namespace StudentsSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using StudentsSystem.Services.Data;
    using StudentsSystem.Web.ViewModels.Course;

    public class CourseController : BaseController
    {
        private readonly ICoursesService coursesService;

        public CourseController(ICoursesService coursesService)
        {
            this.coursesService = coursesService;
        }

        public IActionResult All()
        {
            var allCourses = this.coursesService.GetAllCourses();

            var viewModel = new AllViewModel
            {
                Courses = allCourses,
            };

            return this.View(viewModel);
        }
    }
}

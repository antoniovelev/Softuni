namespace StudentsSystem.Web.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using StudentsSystem.Data.Models;
    using StudentsSystem.Services.Data;
    using StudentsSystem.Services.Mapping;
    using StudentsSystem.Web.ViewModels.Course;

    public class CourseController : BaseController
    {
        private readonly ICoursesService coursesService;
        private readonly UserManager<ApplicationUser> userManager;

        public CourseController(
            ICoursesService coursesService,
            UserManager<ApplicationUser> userManager)
        {
            this.coursesService = coursesService;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public IActionResult All()
        {
            var allCourses = this.coursesService.GetAllCourses();

            var viewModel = new AllViewModel
            {
                Courses = allCourses,
            };

            return this.View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateInputModel inputModel)
        {
            var grade = double.Parse(inputModel.Grade);
            if ((!this.ModelState.IsValid)
                || grade > 6.00
                || grade < 2.00)
            {
                return this.View(inputModel);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            await this.coursesService.CreateAsync(user.Id, inputModel);

            return this.Redirect("/Course/All");
        }
    }
}

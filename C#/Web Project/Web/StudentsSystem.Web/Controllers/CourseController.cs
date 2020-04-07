namespace StudentsSystem.Web.Controllers
{
    using System.Globalization;
    using System.Linq;
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
        public async Task<IActionResult> All()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var allCourses = this.coursesService.GetAllCourses().Where(s => s.UserId == user.Id);

            var viewModel = new AllViewModel
            {
                Courses = user.Courses,
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

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            await this.coursesService.CreateAsync(user.Id, inputModel);

            return this.Redirect("/Course/All");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Details(string id)
        {
            var viewModel = this.coursesService.GetCourseById<DetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }
    }
}

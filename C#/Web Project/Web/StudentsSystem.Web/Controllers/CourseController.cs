namespace StudentsSystem.Web.Controllers
{
    using System;
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
        private const int RecordsPerPage = 5;
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
        public async Task<IActionResult> All(int? page = 1)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var allCourses = this.coursesService.GetAllCourses(user.Id, RecordsPerPage, (int)((page - 1) * RecordsPerPage));

            var viewModel = new AllViewModel
            {
                Courses = allCourses,
                CurrentPage = (int)page,
            };
            var count = this.coursesService.GetCount(user.Id);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / RecordsPerPage);
            return this.View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Old(int? page = 1)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var allCourses = this.coursesService.GetAllOldCourses(user.Id, RecordsPerPage, (int)((page - 1) * RecordsPerPage));
            var viewModel = new AllViewModel
            {
                Courses = allCourses.Where(x => x.Grade.HasValue),
                CurrentPage = (int)page,
            };
            var count = this.coursesService.GetOldCoursesCount(user.Id);
            viewModel.PagesCount = (int)Math.Ceiling((double)count / RecordsPerPage);
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
            viewModel.StartOn = Convert.ToDateTime(viewModel.StartOn).ToString("dd-MM-yyyy");
            viewModel.EndOn = Convert.ToDateTime(viewModel.EndOn).ToString("dd-MM-yyyy");

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult OldCourseDetails(string id)
        {
            var viewModel = this.coursesService.GetCourseById<DetailsViewModel>(id);
            viewModel.StartOn = Convert.ToDateTime(viewModel.StartOn).ToString("dd-MM-yyyy");
            viewModel.EndOn = Convert.ToDateTime(viewModel.EndOn).ToString("dd-MM-yyyy");
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(string courseId)
        {
            var inputModel = this.coursesService.GetCourseById<EditInputModel>(courseId);
            inputModel.StartOn = Convert.ToDateTime(inputModel.StartOn).ToString("dd-MM-yyyy");
            inputModel.EndOn = Convert.ToDateTime(inputModel.EndOn).ToString("dd-MM-yyyy");
            return this.View(inputModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(string courseId, EditInputModel inputModel)
        {
            var course = this.coursesService.GetById(courseId);
            inputModel.Id = course.Id;
            inputModel.UserUserId = course.UserId;
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.coursesService.UpdateAsync(inputModel);
            if (!string.IsNullOrWhiteSpace(inputModel.Grade))
            {
                return this.Redirect("/Course/OldCourseDetails?id=" + inputModel.Id);
            }

            return this.Redirect("/Course/Details?id=" + inputModel.Id);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            await this.coursesService.DeleteByIdAsync(id);
            return this.Redirect("/Course/All");
        }
    }
}

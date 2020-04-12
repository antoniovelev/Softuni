namespace StudentsSystem.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using StudentsSystem.Data.Models;
    using StudentsSystem.Services.Data;
    using StudentsSystem.Web.ViewModels.Homework;

    public class HomeworkController : BaseController
    {
        private readonly IHomeworksService homeworksService;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeworkController(
            IHomeworksService homeworksService,
            UserManager<ApplicationUser> userManager)
        {
            this.homeworksService = homeworksService;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var homeworks = this.homeworksService.GetAllHomeworks().Where(x => x.UserId == user.Id);

            var viewModel = new AllViewModel
            {
                Homeworks = homeworks,
            };
            return this.View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create(string courseId)
        {
            var inputModel = new CreateInputModel
            {
                CourseId = courseId,
            };

            return this.View(inputModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(string courseId, CreateInputModel inputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            await this.homeworksService.CreateHomeworkAsync(user.Id, inputModel);
            return this.Redirect("/Homework/All");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Details(string id)
        {
            var viewModel = this.homeworksService.GetHomeworkById<DetailsViewModel>(id);
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }
    }
}

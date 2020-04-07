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
    using StudentsSystem.Web.ViewModels.Event;

    public class EventController : BaseController
    {
        private readonly IEventsService eventsService;
        private readonly ICoursesService coursesService;
        private readonly UserManager<ApplicationUser> userManager;

        public EventController(IEventsService eventsService, ICoursesService coursesService, UserManager<ApplicationUser> userManager)
        {
            this.eventsService = eventsService;
            this.coursesService = coursesService;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var events = this.eventsService.GetAllEvents().Where(x => x.CourseId == user.Courses.Select(y => y.Id).FirstOrDefault());

            var viewModel = new AllViewModel
            {
                Events = events,
            };

            return this.View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create(string courseId)
        {
            var model = new CreateInputModel()
            {
                CourseId = courseId,
            };
            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePost(string courseId, CreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            inputModel.CourseId = courseId;
            await this.eventsService.CreateEventAsync(inputModel);

            return this.Redirect("/Event/All");
        }
    }
}

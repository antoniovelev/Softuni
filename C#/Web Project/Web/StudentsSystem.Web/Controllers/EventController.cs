namespace StudentsSystem.Web.Controllers
{
    using System;
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
        private readonly UserManager<ApplicationUser> userManager;

        public EventController(IEventsService eventsService, UserManager<ApplicationUser> userManager)
        {
            this.eventsService = eventsService;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var events = this.eventsService.GetAllEvents().Where(x => x.UserId == user.Id);

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
            if (courseId == null)
            {
                return this.NotFound("Somew");
            }

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

            var user = await this.userManager.GetUserAsync(this.User);
            inputModel.CourseId = courseId;
            await this.eventsService.CreateEventAsync(user.Id, inputModel);

            return this.Redirect("/Event/All");
        }

        [Authorize]
        [HttpGet]
        public IActionResult Details(string id)
        {
            var viewModel = this.eventsService.GetEventById<DetailsViewModel>(id);
            viewModel.Date = Convert.ToDateTime(viewModel.Date).ToString("dd-MM-yyyy");
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Edit(string eventId)
        {
            var inputModel = this.eventsService.GetEventById<EditInputModel>(eventId);

            inputModel.Date = Convert.ToDateTime(inputModel.Date).ToString("dd-MM-yyyy");
            return this.View(inputModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(string eventId, EditInputModel inputModel)
        {
            inputModel.Id = eventId;
            var eventById = this.eventsService.GetEventById(eventId);
            inputModel.CourseId = eventById.CourseId;

            if (!this.ModelState.IsValid)
            {
                return this.NotFound(inputModel);
            }

            await this.eventsService.UpdateAsync(inputModel);
            var updateEvent = this.eventsService.GetEventById(inputModel.Id);
            return this.Redirect("/Event/Details?id=" + updateEvent.Id);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            await this.eventsService.DeleteByIdAsync(id);
            return this.Redirect("/Event/All");
        }
    }
}

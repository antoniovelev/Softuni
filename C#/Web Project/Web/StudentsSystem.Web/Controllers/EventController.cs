namespace StudentsSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using StudentsSystem.Services.Data;
    using StudentsSystem.Web.ViewModels.Event;

    public class EventController : BaseController
    {
        private readonly IEventsService eventsService;

        public EventController(IEventsService eventsService)
        {
            this.eventsService = eventsService;
        }

        [HttpGet]
        public IActionResult All()
        {
            var events = this.eventsService.GetAllEvents();

            var viewModel = new AllViewModel
            {
                Events = events,
            };

            return this.View(viewModel);
        }
    }
}

namespace StudentsSystem.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using StudentsSystem.Services.Data;
    using StudentsSystem.Web.ViewModels.Event;

    public class EventContorller : BaseController
    {
        private readonly IEventsService eventsService;

        public EventContorller(IEventsService eventsService)
        {
            this.eventsService = eventsService;
        }

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

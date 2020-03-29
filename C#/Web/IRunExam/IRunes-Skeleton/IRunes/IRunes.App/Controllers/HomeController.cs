using IRunes.App.ViewModels.Home;
using IRunes.Services;
using SIS.HTTP;
using SIS.MvcFramework;

namespace IRunes.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUsersService usersService;

        public HomeController(IUsersService usersService)
        {
            this.usersService = usersService;
        }


        [HttpGet("/")]
        public HttpResponse IndexSlash()
        {
            return this.Index();
        }

        public HttpResponse Index()
        {
            var viewModel = new UserViewModel() { Username = this.usersService.GetUsernameById(User)};

            if (this.IsUserLoggedIn())
            {
                return this.View(viewModel, "/Home");
            }
            else
            {
                return this.View();
            }
        }
    }
}

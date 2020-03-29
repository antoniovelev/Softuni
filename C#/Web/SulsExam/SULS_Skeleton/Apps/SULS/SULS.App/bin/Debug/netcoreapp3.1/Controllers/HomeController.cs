using SIS.HTTP.Responses;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;

namespace SULS.App.Controllers
{
    public class HomeController : Controller
    {
        public HttpResponse Index()
        {
            if (this.IsLoggedIn())
            {
                return this.Redirect("/IndexLoggedIn");
            }
            return this.View();
        }

        [HttpGet(Url = "/")]     
        public HttpResponse IndexSlash()
        {
            return this.Index();
        }
       
    }
}
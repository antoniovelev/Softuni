using Panda.Services;
using Panda.Web.ViewModels.Users;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using System;
using System.Collections.Generic;
using System.Text;

namespace Panda.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }
        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(LoginInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Users/Login");
            }

            var user = this.userService.GetUser(inputModel.Username, inputModel.Password);

            if (user == null)
            {
                this.Redirect("/Users/Login");
            }

            this.SignIn(user.Id, user.Username, user.Email);
            return this.Redirect("/");
        }
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return this.Redirect("/Users/Register");
            }

            if (input.Password != input.ConfirmPassword)
            {
                return this.Redirect("/Users/Register");
            }

            var userId = this.userService.CreateUser(input.Username, input.Email, input.Password);
            this.SignIn(userId, input.Username, input.Email);

            return this.Redirect("/");
        }

        [Authorize]
        public IActionResult Logout()
        {
            this.SignOut();
            return this.Redirect("/");
        }
    }
}

using Panda.Data.Models.Enums;
using Panda.Services;
using Panda.Web.ViewModels.Packages;
using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Panda.Web.Controllers
{
    public class PackagesController : Controller
    {
        private readonly IPackagesService packagesService;
        private readonly IUserService userService;

        public PackagesController(IPackagesService packagesService, IUserService userService)
        {
            this.packagesService = packagesService;
            this.userService = userService;
        }

        [Authorize]
        public IActionResult Create()
        {
            var usernames = this.userService.GetUsernames();

            return this.View(usernames);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(CreateInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Packages/Create");
            }

            this.packagesService.Create(inputModel.Description, inputModel.Weight, inputModel.ShippingAddress, inputModel.RecipientName);
            return this.Redirect("/Packages/Pending");
        }

        [Authorize]
        public IActionResult Delivered()
        {
            var packages = this.packagesService.GetAllByStatus(Status.Delivered)
                .Select(x => new PackageViewModel 
                {
                    Description = x.Description,
                    Id = x.Id,
                    Weight = x.Weight,
                    ShippingAddress = x.ShippingAddress,
                    RecipientName = x.Recipient.Username
                }).ToList();

            return this.View(new PackagesListViewModel { Packages = packages});
        }

        [Authorize]
        public IActionResult Pending()
        {
            var packages = this.packagesService.GetAllByStatus(Status.Pending)
                .Select(x => new PackageViewModel
                {
                    Description = x.Description,
                    Id = x.Id,
                    Weight = x.Weight,
                    ShippingAddress = x.ShippingAddress,
                    RecipientName = x.Recipient.Username
                }).ToList();

            return this.View(new PackagesListViewModel{ Packages = packages});
        }

        [Authorize]
        public IActionResult Deliver(string id)
        {
            this.packagesService.Deliver(id);
            return this.Redirect("/Packages/Delivered");
        }
    }
}

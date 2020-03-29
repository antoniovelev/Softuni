using Andreys.Services;
using Andreys.ViewModels.Products;
using SIS.HTTP;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Andreys.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductsService productsService;

        public ProductController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [HttpGet(url:"/Products/Add")]
        public HttpResponse Add()
        
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }
            return this.View();
        }

        //[HttpPost]
        [HttpPost(url: "/Products/Add")]
        public HttpResponse Add(AddInputModel inputModel)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (inputModel.Name.Length < 4 || inputModel.Name.Length > 20)
            {
                return this.Redirect("/Products/Add");
            }

            if (string.IsNullOrWhiteSpace(inputModel.Description) || inputModel.Description.Length > 10)
            {
                return this.Redirect("/Products/Add");
            }

            this.productsService.Add(inputModel.Name, inputModel.Description,
                inputModel.ImageURL, inputModel.Category, inputModel.Gender, inputModel.Price);

            return this.Redirect("/Home");
        }
    }
}

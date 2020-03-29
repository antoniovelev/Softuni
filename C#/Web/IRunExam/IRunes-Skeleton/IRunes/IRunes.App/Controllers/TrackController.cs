using IRunes.App.ViewModels.Tracks;
using IRunes.Services;
using SIS.HTTP;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.App.Controllers
{
    public class TrackController : Controller
    {
        private readonly ITracksService tracksService;

        public TrackController(ITracksService tracksService)
        {
            this.tracksService = tracksService;
        }

        public HttpResponse Create(string albumId)
        {
            var viewModel = new CreateViewModel { AlbumId = albumId };
            return this.View(viewModel);
        }

        [HttpPost]
        public HttpResponse Create(CreateInputModel inputModel)
        {
            if (!IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (inputModel.Name.Length <= 4 || inputModel.Name.Length >= 20)
            {
                return this.Redirect("/Tracks/Create");
            }

            if (string.IsNullOrWhiteSpace(inputModel.Link))
            {
                return this.Redirect("/Tracks/Create");
            }

            if (!inputModel.Link.StartsWith("http"))
            {
                return this.Redirect("/Tracks/Create");
            }

            if (inputModel.Price < 0.00M)
            {
                return this.Redirect("/Tracks/Create");
            }

            return this.View();
        }

        //[HttpPost]
        //public HttpResponse Create(TrackInputModel inputModel)
        //{
        //    if (!IsUserLoggedIn())
        //    {
        //        return this.Redirect("/Users/Login");
        //    }

        //    if (inputModel.Name.Length <= 4 && inputModel.Name.Length >= 20)
        //    {
        //        return this.Redirect("/Tracks/Create");
        //    }

        //    if (string.IsNullOrWhiteSpace(inputModel.Link))
        //    {
        //        return this.Redirect("/Tracks/Create");
        //    }

        //    if (inputModel.Price < 0.00M)
        //    {
        //        return this.Redirect("/Tracks/Create");
        //    }
        //    this.tracksService.Create(inputModel.Name, inputModel.Link, inputModel.Price);

        //    return this.View();
        //}
    }
}

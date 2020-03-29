using IRunes.App.ViewModels.Albums;
using IRunes.App.ViewModels.Tracks;
using IRunes.Models;
using IRunes.Services;
using SIS.HTTP;
using SIS.MvcFramework;
using System.Collections.Generic;
using System.Linq;

namespace IRunes.App.Controllers
{
   public class AlbumsController : Controller
    {
        private readonly IAlbumsService albumsService;

        public AlbumsController(IAlbumsService albumsService)
        {
            this.albumsService = albumsService;
        }
        public HttpResponse All()
        {
            if (!IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            List<Album> albums = this.albumsService.GetAllAlbums();
            var list = new List<AlbumInfoViewModel>();
            foreach (var item in albums)
            {
                var curr = new AlbumInfoViewModel { Id = item.Id, Name = item.Name };
                list.Add(curr);
            }
            var viewModel = new AllAlbumsViewModel() 
            { 
                Albums = list
            };

            return this.View(viewModel, "/All");
        }

        public HttpResponse Create()
        {
            if (!IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Create(CreateInputModel inputModel)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (inputModel.Name.Length <= 4 && inputModel.Name.Length >= 20)
            {
                return this.Redirect("/Albums/Create");
            }

            if (string.IsNullOrWhiteSpace(inputModel.Cover))
            {
                return this.Redirect("/Albums/Create");
            }

            albumsService.Create(inputModel.Name, inputModel.Cover);
            return this.Redirect("/Albums/All");
        }

        public HttpResponse Details(string id)
        {
            if (!IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var album = this.albumsService.GetAlbum(id);

            if (album == null)
            {
                return this.Redirect("/Albums/All");
            }

            var viewModel = new DetailsViewMode()
            {
                Id = album.Id,
                Name = album.Name,
                Cover = album.Cover,
                Price = (album.Tracks.Sum(x => x.Price)) * 0.87M,
                Tracks = album.Tracks.Select(t => new TracksInfoViewModel
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList()
            };
            return this.View(viewModel);
        }
    }
}

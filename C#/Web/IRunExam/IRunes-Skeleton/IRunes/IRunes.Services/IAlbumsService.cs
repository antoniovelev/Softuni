using IRunes.Models;
using System.Collections.Generic;

namespace IRunes.Services
{
    public interface IAlbumsService
    {
        void Create(string name, string cover);

        List<Album> GetAllAlbums();

        Album GetAlbum(string id);

    }
}

using IRunes.Data;
using IRunes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IRunes.Services
{
    public class TracksService : ITracksService
    {
        private readonly RunesDbContext db;

        public TracksService(RunesDbContext db)
        {
            this.db = db;
        }
        public void Create(string albumId, string name, string link, decimal price)
        {
            var track = new Track
            {
                Id = albumId,
                Name = name,
                Link = link,
                Price = price
            };

            this.db.Tracks.Add(track);

            var tracks = this.db.Tracks.Where(x => x.Id == albumId)
                .Select(t => t.Price).ToList();

            this.db.SaveChanges();
        }
    }
}

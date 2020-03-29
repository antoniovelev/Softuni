using IRunes.Data;
using IRunes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace IRunes.Services
{
    public class UsersService : IUsersService
    {
        private readonly RunesDbContext db;

        public UsersService(RunesDbContext db)
        {
            this.db = db;
        }

        public User GetUser(string username, string password)
        {
            var hashPassword = HashPassword(password);
            var user = db.Users.FirstOrDefault(x => x.Username == username && x.Password == hashPassword);

            return user;
        }

        public string GetUsernameById(string id)
        {
            var username = this.db.Users.Where(s=>s.Id==id).Select(e=>e.Username).FirstOrDefault();

            return username;
        }

        public void Register(string username, string password, string email)
        {
            var user = new User
            {
                Username = username,
                Password = HashPassword(password),
                Email = email
            };

            this.db.Users.Add(user);
            this.db.SaveChanges();
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return Encoding.UTF8.GetString(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }
    }
}

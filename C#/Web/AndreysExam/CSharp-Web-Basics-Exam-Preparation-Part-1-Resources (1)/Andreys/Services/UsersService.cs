using Andreys.Data;
using Andreys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Andreys.Services
{
    public class UsersService : IUsersService
    {
        private readonly AndreysDbContext db;

        public UsersService(AndreysDbContext db)
        {
            this.db = db;
        }

        public bool EmailExists(string email)
        {
            var user = this.db.Users.FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string GetUserId(string username, string password)
        {
            var hashPassword = HashPassword(password);
            var user = this.db.Users.FirstOrDefault(x => x.Username == username && x.Password == hashPassword);

            return user.Id;
        }

        public void Register(string username, string email, string password)
        {
            var user = new User
            {
                Username = username,
                Email = email,
                Password = HashPassword(password)
            };

            db.Users.Add(user);
            db.SaveChanges();
        }

        public bool UsernameExists(string username)
        {
            var user = this.db.Users.FirstOrDefault(x => x.Username == username);

            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
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

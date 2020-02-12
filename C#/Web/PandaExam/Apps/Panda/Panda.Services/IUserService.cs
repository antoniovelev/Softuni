using Panda.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Panda.Services
{
    public interface IUserService
    {
        string CreateUser(string username, string email, string password);

        User GetUser(string username, string password);

        IEnumerable<string> GetUsernames();
    }
}

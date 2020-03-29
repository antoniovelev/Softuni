using SULS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SULS.Services
{
    public interface IUsersService
    {
        void Register(string username, string email, string password);

        User GetUser(string username, string password);
    }
}


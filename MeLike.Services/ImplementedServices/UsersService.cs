using MeLike.Services.Interfaces;
using MeLike.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeLike.Services.ImplementedServices
{
    public class UsersService : IUsersService
    {
        // TODO
        public UserViewModel User { get; set; } = new UserViewModel { Login = "BigBoss" };
    }
}

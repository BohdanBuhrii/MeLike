using MeLike.Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeLike.Services.Interfaces
{
    public interface IUsersService
    {
        public UserViewModel User { get; set; }
    }
}

using System.Collections.Generic;

namespace MeLike.Services.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public string HashPassword { get; set; }

        public List<string> Friends { get; set; }
    }
}

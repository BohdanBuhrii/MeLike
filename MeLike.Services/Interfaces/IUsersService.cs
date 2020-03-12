using MeLike.Services.ViewModels;
using System.Threading.Tasks;

namespace MeLike.Services.Interfaces
{
    public interface IUsersService
    {
        public UserViewModel User { get; set; }

        public Task<UserViewModel> GetUserByEmail(string email);
    }
}

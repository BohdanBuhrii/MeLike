using MeLike.Data.Graph.Enums;
using MeLike.Services.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeLike.Services.Interfaces
{
    public interface IUsersService
    {
        UserViewModel User { get; set; }

        Task<IEnumerable<UserViewModel>> GetAllUsers(PageViewModel page, bool includeConnections = false);

        Task<UserViewModel> GetUserByEmail(string email);

        Task<UserViewModel> GetUserByLogin(string login);

        Task AddFriend(string friendLogin);

        Task DeleteFriend(string friendLogin);

        Task RenameUser(string newName);

        Task<ConnectionType> FindConnectionTo(UserViewModel user);
    }
}

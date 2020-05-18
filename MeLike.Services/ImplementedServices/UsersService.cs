using AutoMapper;
using MeLike.Data.Entities;
using MeLike.Data.Graph.Enums;
using MeLike.Data.Graph.Interfaces;
using MeLike.Data.Graph.Nodes;
using MeLike.Data.Interfaces;
using MeLike.Services.Interfaces;
using MeLike.Services.ViewModels;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeLike.Services.ImplementedServices
{
    public class UsersService : IUsersService
    {
        private readonly IMeLikeContext _context;
        private readonly IMongoQueryable<User> _users;
        private readonly IUserConnectionsRepository _connectionsRepository;
        private readonly IMapper _mapper;

        public UsersService(IMeLikeContext context, IUserConnectionsRepository connectionsRepository, IMapper mapper)
        {
            _context = context;
            _connectionsRepository = connectionsRepository;
            _mapper = mapper;
            _users = _context.Users.AsQueryable();
        }

        public UserViewModel User { get; set; }

        public async Task<IEnumerable<UserViewModel>> GetAllUsers(PageViewModel page, bool includeConnections = false)
        {
            var users = await _users
                .Where(u => u.Login != User.Login)
                .OrderBy(u => u.Login)
                .Skip(page.Number * page.Size)
                .Take(page.Size)
                .ToListAsync();

            var viewModels = _mapper.Map<IEnumerable<UserViewModel>>(users);

            if (includeConnections)
            {
                foreach (var model in viewModels)
                {
                    model.ConnectionType = await FindConnectionTo(model);
                }
            }

            return viewModels;
        }

        public async Task<UserViewModel> GetUserByEmail(string email)
        {
            return _mapper.Map<UserViewModel>(
                await _users.FirstOrDefaultAsync(u => u.Email == email));
        }

        public async Task<UserViewModel> GetUserByLogin(string login)
        {
            return _mapper.Map<UserViewModel>(
                await _users.FirstOrDefaultAsync(u => u.Login == login));
        }

        public async Task AddFriend(string friendLogin)
        {
            var setter = Builders<User>.Update.Push(el => el.Friends, friendLogin);

            await _context.Users.UpdateOneAsync(el => el.Id == User.Id, setter);
            
            User.Friends.Add(friendLogin);

            await _connectionsRepository.AddFollower(
                _mapper.Map<UserNode>(User),
                _mapper.Map<UserNode>(await GetUserByLogin(friendLogin)));
        }

        public async Task DeleteFriend(string friendLogin)
        {
            var setter = Builders<User>.Update.Pull(el => el.Friends, friendLogin);

            await _context.Users.UpdateOneAsync(el => el.Id == User.Id, setter);

            User.Friends.Remove(friendLogin);

            await _connectionsRepository.RemoveFollower(
                _mapper.Map<UserNode>(User),
                _mapper.Map<UserNode>(await GetUserByLogin(friendLogin)));
        }

        public async Task RenameUser(string newName)
        {
            var changeLog = new UserNameChangeLog { Old = User.Login, New = newName };
            var setter = Builders<User>.Update.Set(el => el.Login, newName);

            await _context.Users.UpdateOneAsync(el => el.Id == User.Id, setter);
            await _context.UserNameChangeLogs.InsertOneAsync(changeLog);

            User.Login = newName;
        }

        public async Task<ConnectionType> FindConnectionTo(UserViewModel user)
        {
            var path = await _connectionsRepository.GetConnectingPath(
                _mapper.Map<UserNode>(User),
                _mapper.Map<UserNode>(user),
                length: 3);

            if (path == null)
            {
                return ConnectionType.Other;
            }

            return  (ConnectionType) (path.Count() - 1);


        }
    }
}

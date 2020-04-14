using AutoMapper;
using MeLike.Data.Entities;
using MeLike.Data.Interfaces;
using MeLike.Services.Interfaces;
using MeLike.Services.ViewModels;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeLike.Services.ImplementedServices
{
    public class UsersService : IUsersService
    {
        private readonly IMeLikeContext _context;
        private readonly IMongoQueryable<User> _users;
        private readonly IMapper _mapper;

        public UsersService(IMeLikeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _users = _context.Users.AsQueryable();
        }

        public UserViewModel User { get; set; }

        public async Task<IEnumerable<UserViewModel>> GetAllUsers(PageViewModel page)
        {
            var posts = await _users
                .Where(u => u.Login != User.Login)
                .OrderBy(u => u.Login)
                .Skip(page.Number * page.Size)
                .Take(page.Size)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UserViewModel>>(posts);
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
        }

        public async Task DeleteFriend(string friendLogin)
        {
            var setter = Builders<User>.Update.Pull(el => el.Friends, friendLogin);

            await _context.Users.UpdateOneAsync(el => el.Id == User.Id, setter);

            User.Friends.Remove(friendLogin);
        }

        public async Task RenameUser(string newName)
        {
            var changeLog = new UserNameChangeLog { Old = User.Login, New = newName };
            var setter = Builders<User>.Update.Set(el => el.Login, newName);

            await _context.Users.UpdateOneAsync(el => el.Id == User.Id, setter);
            await _context.UserNameChangeLogs.InsertOneAsync(changeLog);
        }
    }
}

using MeLike.Data.Entities;
using MeLike.Data.Interfaces;
using MeLike.Services.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeLike.Services.ImplementedServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMeLikeContext _context;

        public AuthenticationService(IMeLikeContext context)
        {
            _context = context;
        }

        public async Task<bool> IsCredentialsValid(string email, string password)
        {
            /*await _context.Users.InsertOneAsync(new User
            {
                Email = email,
                HashPassword = password,
                Friends = new List<string>(),
                Login = email
            });*/

            return await _context.Users
                .AsQueryable()
                .Where(u => u.Email == email && u.HashPassword == Hash(password))
                .AnyAsync();
        }

        private string Hash(string password)
        {
            // TODO Password Hashing
            return password;
        }
    }
}

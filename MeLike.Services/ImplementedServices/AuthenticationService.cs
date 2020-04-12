using MeLike.Data.Interfaces;
using MeLike.Services.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
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

using AutoMapper;
using MeLike.Data.Entities;
using MeLike.Data.Interfaces;
using MeLike.Services.Interfaces;
using MeLike.Services.ViewModels;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
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

        public async Task<UserViewModel> GetUserByEmail(string email)
        {
            return _mapper.Map<UserViewModel>(
                await _users.FirstOrDefaultAsync(u => u.Email == email));
        }

        public async Task a(string paramUsername, string paramPassword) {
            try
            {
                // Clear the existing external cookie
                await HttpContext
                    .SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch { }
            // *** !!! This is where you would validate the user !!! ***
            // In this example we just log the user in
            // (Always log the user in for this demo)
            var claims = new List<Claim>
    {
                new Claim(ClaimTypes.Name, paramUsername),
                new Claim(ClaimTypes.Role, "Administrator"),
            };
            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                RedirectUri = this.Request.Host.Value
            };
            try
            {
                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return LocalRedirect(returnUrl);

        }

    }
}

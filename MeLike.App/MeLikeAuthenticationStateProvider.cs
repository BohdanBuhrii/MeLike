using MeLike.Services.Interfaces;
using MeLike.Services.ViewModels;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MeLike.Authentication
{
    public class MeLikeAuthenticationStateProvider : AuthenticationStateProvider
    {
        private AuthenticationState authenticationState
            = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        private readonly IUsersService _usersService;
        private readonly IAuthenticationService _authService;

        public MeLikeAuthenticationStateProvider(IUsersService usersService, IAuthenticationService authService) : base()
        {
            _usersService = usersService;
            _authService = authService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // TODO cookies
            var user = await _usersService.GetUserByEmail("testemail@gmail.com");
            await SetUpNewUser(user.Email, user.HashPassword);

            return authenticationState;
        }

        public async Task SetUpNewUser(string email, string password)
        {
            ClaimsIdentity identity;

            if (await _authService.IsCredentialsValid(email, password))
            {
                _usersService.User = await _usersService.GetUserByEmail(email); ;

                identity = new ClaimsIdentity(new List<Claim>
                        {
                            new Claim(ClaimTypes.Email, _usersService.User.Email),
                            new Claim(ClaimTypes.Name, _usersService.User.Login)

                        }, "ServerAuth");
            }
            else
            {
                identity = new ClaimsIdentity();
                _usersService.User = new UserViewModel();
            }

            authenticationState = new AuthenticationState(new ClaimsPrincipal(identity));
            NotifyAuthenticationStateChanged(Task.FromResult(authenticationState));
        }
    }
}

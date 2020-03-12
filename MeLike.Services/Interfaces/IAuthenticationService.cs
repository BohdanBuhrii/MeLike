using System.Threading.Tasks;

namespace MeLike.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<bool> IsCredentialsValid(string email, string password);
    }
}

using MeLike.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MeLike.Data
{
    public static class Configuration
    {
        public static void ConfigureDataAccess(this IServiceCollection services)
        {
            services.AddSingleton<IMeLikeContext, MeLikeContext>();
        }
    }
}

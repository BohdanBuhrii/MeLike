using MeLike.Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MeLike.Data.Configuration
{
    public static class Configuration
    {
        public static void ConfigureDataAccess(this IServiceCollection services)
        {
            services.AddSingleton(c => new ClientConfiguration
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "melikedb"
            });
            services.AddSingleton<IMeLikeContext, MeLikeContext>();
        }
    }

    public class ClientConfiguration
    {
        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}

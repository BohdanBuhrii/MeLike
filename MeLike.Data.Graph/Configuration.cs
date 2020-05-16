using MeLike.Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using Neo4jClient;
using System;

namespace MeLike.Data.Graph.Configuration
{
    public static class Configuration
    {
        public static void ConfigureGraphAccess(this IServiceCollection services)
        {
            services.AddSingleton(c => 
                NeoServerConfiguration.GetConfiguration(new Uri("http://localhost:7474/db/data"), "neo4j", "MeLikeGraph"
            ));

            services.AddSingleton<IGraphClientFactory, GraphClientFactory>();
        }
    }
}

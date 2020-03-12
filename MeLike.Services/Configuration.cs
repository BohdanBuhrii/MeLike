using AutoMapper;
using MeLike.Data;
using MeLike.Data.Entities;
using MeLike.Services.ImplementedServices;
using MeLike.Services.Interfaces;
using MeLike.Services.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace MeLike.Services
{
    public static class Configuration
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.ConfigureDataAccess();
            services.ConfigureMapper();
            services.AddScoped<IPostsService, PostsService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
        }

        private static void ConfigureMapper(this IServiceCollection services)
        {
            services.AddSingleton(
                new MapperConfiguration(cfg => 
                {
                    cfg.CreateMap<Post, PostViewModel>()
                        .ReverseMap();
                    cfg.CreateMap<Comment, CommentViewModel>()
                        .ReverseMap();
                    cfg.CreateMap<Emotion, EmotionViewModel>()
                        .ReverseMap();
                    cfg.CreateMap<User, UserViewModel>()
                        .ReverseMap();
                }).CreateMapper()
            );
        }
    }
}

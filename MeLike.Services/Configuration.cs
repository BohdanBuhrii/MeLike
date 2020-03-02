using AutoMapper;
using MeLike.Data;
using MeLike.Data.Entities;
using MeLike.Services.ImplementedServices;
using MeLike.Services.Interfaces;
using MeLike.Services.Authentication;
using MeLike.Services.ViewModels;
using Microsoft.AspNetCore.Identity;
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
            //services.AddScoped<IUserStore<UserViewModel>, MeLikeUserStore>();

            services.AddScoped(typeof(IUserStore<>), typeof(MeLikeUserStore<>));

            services.AddScoped(typeof(IPasswordHasher<>), typeof(Authentication.PasswordHasher<>));
            // note you can use default hasher
            
            //services.AddScoped<UserManager<UserViewModel>>();
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

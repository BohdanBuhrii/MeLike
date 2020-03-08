using MeLike.Data.Enums;
using MeLike.Services.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeLike.Services.Interfaces
{
    public interface IPostsService
    {
        Task<PostViewModel> GetPostById(string postId);

        Task<IEnumerable<PostViewModel>> GetAllPosts();

        Task<IEnumerable<PostViewModel>> GetPostsByUserLogin(string userLogin);

        Task<IEnumerable<PostViewModel>> GetPostsByUserFriends();

        Task<PostViewModel> FetchChanges(PostViewModel post);

        Task Create(PostViewModel post);

        Task AddEmotion(PostViewModel post, EmotionType emotion);

        Task AddComment(PostViewModel post, string comment);
    }
}

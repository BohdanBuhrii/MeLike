using MeLike.Data.Enums;
using MeLike.Services.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeLike.Services.Interfaces
{
    public interface IPostsService
    {
        Task<PostViewModel> GetPostById(string postId);

        Task<IEnumerable<PostViewModel>> GetAllPosts(PageViewModel page);

        Task<IEnumerable<PostViewModel>> GetPostsByUserLogin(string userLogin, PageViewModel page);

        Task<IEnumerable<PostViewModel>> GetPostsByUserFriends(PageViewModel page);

        Task<PostViewModel> FetchChanges(PostViewModel post);

        Task Create(PostViewModel post);

        Task AddEmotion(PostViewModel post, EmotionType emotion);

        Task AddComment(PostViewModel post, string comment);
    }
}

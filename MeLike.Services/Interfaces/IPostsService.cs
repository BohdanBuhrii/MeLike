using MeLike.Services.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeLike.Services.Interfaces
{
    public interface IPostsService
    {
        Task<IEnumerable<PostViewModel>> GetAllPosts();

        Task Create(PostViewModel post);
    }
}

using MeLike.Data.Entities;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace MeLike.Data.Interfaces
{
    public interface IMeLikeContext
    {
        IMongoCollection<Post> Posts { get; }

        IMongoCollection<User> Users { get; }

        Task SaveChangesAsync();
    }
}

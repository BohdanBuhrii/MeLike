using System.Threading.Tasks;
using MeLike.Data.Configuration;
using MeLike.Data.Entities;
using MeLike.Data.Interfaces;
using MongoDB.Driver;

namespace MeLike.Data
{
    public class MeLikeContext : IMeLikeContext
    {
        private IMongoCollection<Post> _posts;
        private IMongoCollection<User> _users;
        private readonly IMongoDatabase _database;

        public IMongoCollection<Post> Posts
        {
            get 
            {
                if (_posts == null)
                {
                    _posts = _database.GetCollection<Post>("posts");
                }
                return _posts;
            }
        }

        public IMongoCollection<User> Users
        {
            get
            {
                if (_users == null)
                {
                    _users = _database.GetCollection<User>("users");
                }
                return _users;
            }
        }

        public MeLikeContext(ClientConfiguration configuration)
        {
            var client = new MongoClient(configuration.ConnectionString);
            _database = client.GetDatabase(configuration.DatabaseName);
        }

        public Task SaveChangesAsync()
        {
            // TODO
            return Task.FromResult(0);
        }
    }
}

using MeLike.Data.Graph.Nodes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MeLike.Data.Graph.Interfaces
{
    public interface IUserConnectionsRepository
    {
        public Task AddUserNode(UserNode user);

        public Task AddFollower(UserNode follower, UserNode target);

        public Task RemoveFollower(UserNode follower, UserNode target);

        public Task<IEnumerable<UserNode>> GetConnectingPath(UserNode left, UserNode right, int length = 3);
    }
}

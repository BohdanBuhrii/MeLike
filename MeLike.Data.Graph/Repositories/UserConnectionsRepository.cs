using Neo4jClient;

namespace MeLike.Data.Graph.Repositories
{
    public class UserConnectionsRepository
    {
        private readonly IGraphClientFactory _clientFactory;

        public UserConnectionsRepository(IGraphClientFactory graphClientFactory)
        {
            _clientFactory = graphClientFactory;
        }


    }
}

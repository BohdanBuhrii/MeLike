using MeLike.Data.Graph.Interfaces;
using MeLike.Data.Graph.Nodes;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeLike.Data.Graph.Repositories
{
    public class UserConnectionsRepository : IUserConnectionsRepository, IDisposable
    {
        private readonly IGraphClientFactory _clientFactory;
        private readonly IGraphClient _client;

        private const string Follows = "FOLLOWS";

        public UserConnectionsRepository(IGraphClientFactory graphClientFactory)
        {
            _clientFactory = graphClientFactory;
            _client = _clientFactory.Create();
        }

        public Task AddUserNode(UserNode user)
        {
            return _client.Cypher
                .Create("(userNode: User {newUser})")
                .WithParam("newUser", user)
                .ExecuteWithoutResultsAsync();
        }

        public Task AddFollower(UserNode follower, UserNode target)
        {
            return _client.Cypher
                .Match("(left: User {Email: {leftEmail}})", "(right: User {Email: {rightEmail}})")
                .WithParam("leftEmail", follower.Email)
                .WithParam("rightEmail", target.Email)
                .Create("(left)-[:" + Follows + "]->(right)")
                .ExecuteWithoutResultsAsync();
        }

        public Task RemoveFollower(UserNode follower, UserNode target)
        {
            return _client.Cypher
                .Match("(left: User {Email: {leftEmail}})-[r:{" + Follows+ "}]->(right: User {Email: {rightEmail}})")
                .WithParam("leftEmail", follower.Email)
                .WithParam("rightEmail", target.Email)
                .Delete("r")
                .ExecuteWithoutResultsAsync();
        }

        public async Task<IEnumerable<UserNode>> GetConnectingPath(UserNode start, UserNode end, int length = 3) {
            var query = await _client.Cypher
                .Match("path = shortestPath((left: User)-[:"+Follows+"*.."+length+"]->(right: User))")
                .Where((UserNode left) => left.Email == start.Email)
                .AndWhere((UserNode right) => right.Email == end.Email)
                .ReturnDistinct<IEnumerable<UserNode>>("[n IN nodes(path) | n]").ResultsAsync;

            return query.FirstOrDefault();
        }

        #region IDisposable

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _client.Dispose();
                }
                
                disposed = true;
            }
        }

        ~UserConnectionsRepository()
        {
            Dispose(false);
        }

        #endregion
    }
}

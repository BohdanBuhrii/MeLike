using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MeLike.Data.Entities;
using MeLike.Data.Enums;
using MeLike.Data.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace MeLikeDBUpdateWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMeLikeContext _context;

        public Worker(ILogger<Worker> logger, IMeLikeContext context)
        {
            _logger = logger;
            _context = context;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                await UpdateUserNames();

                await Task.Delay(5000, stoppingToken);
            }
        }

        private async Task UpdateUserNames()
        {
            while (true)
            {
                var change = await _context.UserNameChangeLogs.FindOneAndDeleteAsync(el => true);

                if (change == null) break;

                _logger.LogInformation($"Rename user {change.Old} to {change.New}");

                await Task.WhenAll(
                    UpdateUserFriends(change),
                    UpdatePostAuthors(change),
                    UpdatePostComments(change),
                    UpdatePostEmotions(change)
                );
            }
        }

        private Task UpdatePostAuthors(UserNameChangeLog change)
        {
            return _context.Posts.UpdateManyAsync(
                el => el.Author == change.Old,
                Builders<Post>.Update.Set(el => el.Author, change.New),
                new UpdateOptions { IsUpsert = false }
            );
        }

        private Task UpdatePostComments(UserNameChangeLog change)
        {
            return _context.Posts.UpdateManyAsync(
                el => el.Comments.Any(c => c.Author == change.Old),
                Builders<Post>.Update.Set("Comments.$[g].Author", change.New),
                new UpdateOptions
                {
                    ArrayFilters = new List<ArrayFilterDefinition>
                    {
                        new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("g.Author", change.Old)),
                    },
                    IsUpsert = false
                }
            );
        }

        private Task UpdatePostEmotions(UserNameChangeLog change)
        {
            return _context.Posts.UpdateManyAsync(
                el => el.Emotions.Any(e => e.Author == change.Old),
                Builders<Post>.Update.Set("Emotions.$[g].Author", change.New),
                new UpdateOptions
                {
                    ArrayFilters = new List<ArrayFilterDefinition>
                    {
                        new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("g.Author", change.Old)),
                    },
                    IsUpsert = false
                }
            );
        }

        private Task UpdateUserFriends(UserNameChangeLog change)
        {
            return _context.Users.UpdateManyAsync(
                el => el.Friends.Contains(change.Old),
                Builders<User>.Update.Set("Friends.$[g]", change.New),
                new UpdateOptions
                {
                    ArrayFilters = new List<ArrayFilterDefinition>
                    {
                        new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("g", change.Old)),
                    },
                    IsUpsert = false
                }
            );
        }
    }
}

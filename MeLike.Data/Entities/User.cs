using MeLike.Data.Interfaces;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace MeLike.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class User : IAuditableEntity
    {
        public string Id { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public string HashPassword { get; set; }

        public List<string> Friends { get; set; }
    }
}

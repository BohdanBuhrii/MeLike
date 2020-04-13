using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MeLike.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class UserNameChangeLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Old { get; set; }

        public string New { get; set; }
    }
}

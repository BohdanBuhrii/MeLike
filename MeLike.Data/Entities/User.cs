using MongoDB.Bson.Serialization.Attributes;

namespace MeLike.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class User
    {
        public string Id { get; set; }
    }
}

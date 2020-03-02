using MongoDB.Bson.Serialization.Attributes;

namespace MeLike.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class Comment
    {
        public string Author { get; set; }

        public string Text { get; set; }
    }
}

using MeLike.Data.Enums;
using MongoDB.Bson.Serialization.Attributes;

namespace MeLike.Data.Entities
{
    [BsonIgnoreExtraElements]
    public class Emotion
    {
        public EmotionType Type { get; set; }

        public string Author { get; set; }
    }
}

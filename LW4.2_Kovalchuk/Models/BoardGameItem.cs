using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LW4._2_Kovalchuk.Models
{
    public enum DifficultyLevel
    {
        Easy = 1,
        Medium = 2,
        Hard = 3
    }
    public class BoardGameItem
    {
        [BsonId]
        public int ?Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("difficulty")]
        public DifficultyLevel Difficulty { get; set; }
        [BsonElement("minPlayers")]
        public int MinPlayers { get; set; }
        [BsonElement("maxPlayers")]
        public int MaxPlayers { get; set; }
    }
    public class BoardGameDTO { 
        public int Id { get; set; }
        public string Name { get; set; }
        public DifficultyLevel Difficulty { get; set; }
        public int MinPlayers { get; set; }
        public int MaxPlayers { get; set; }
    }

}

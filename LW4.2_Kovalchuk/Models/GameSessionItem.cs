using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace LW4._2_Kovalchuk.Models
{
    public class GameSessionItem
    {
        [BsonId]
        public int ?Id { get; set; }
        [BsonElement("numberOfPlayers")]
        public int NumberOfPlayers { get; set; }
        [BsonElement("boardGamesId")]
        public int BoardGameId { get; set; }
        [BsonElement("inProgress")]
        public bool InProgress { get; set; }
        [BsonElement("usersId")]
        public List<int> UserIds { get; set; } = new List<int>();
    }
    public class GameSessionDTO
    {
        public int? Id { get; set; }
        public int NumberOfPlayers { get; set; }
        public int BoardGameId { get; set; }
        public bool InProgress { get; set; }
        public List<int> UserIds { get; set; } = new List<int>();
    }
}

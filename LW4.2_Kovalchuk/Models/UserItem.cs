using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using LW4._2_Kovalchuk.Enum;

namespace LW4._2_Kovalchuk.Models
{
    public class UserItem
    {
        [BsonId]
        public int ?Id { get; set; }
        [BsonElement("username")]
        public string Username { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("passwordHash")]
        public string Password { get; set; }
        [BsonElement("refreshToken")]
        public string? RefreshToken { get; set; }

        [BsonElement("refreshTokenExpiryTime")]
        public DateTime? RefreshTokenExpiryTime { get; set; }
        [BsonElement("role")]
        public Roles Role { get; set; } = Roles.User;
    }
    public class UserDTO
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public Roles Role { get; set; }
    }
}

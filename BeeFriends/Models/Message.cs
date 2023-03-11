using System.Reflection.Metadata;

namespace BeeFriends.Models
{
    public class Message
    {
        public int? Id { get; set; }
        public string? UserMessage { get; set; }
        public string? User { get; set; }
        public int? RoomId { get; set; }
    }
}

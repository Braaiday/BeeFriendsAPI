namespace BeeFriends.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int ChatRoomId { get; set; }
        public ChatRoom ChatRoom { get; set; }
    }
}

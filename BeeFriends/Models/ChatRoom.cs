namespace BeeFriends.Models
{
    public class ChatRoom
    {
        public int ChatRoomId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public List<Message> ?Messages { get; set; }

        public List<User> Users { get; set; }
    }
}

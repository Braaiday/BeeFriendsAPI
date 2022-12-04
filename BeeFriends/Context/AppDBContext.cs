using BeeFriends.Models;
using Microsoft.EntityFrameworkCore;

namespace BeeFriends.Context
{
    public class AppDBContext : DbContext
    {
        public  AppDBContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<ChatRoom> ChatRooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        
    }
}

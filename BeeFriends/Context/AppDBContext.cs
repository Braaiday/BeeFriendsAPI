using BeeFriends.Models;
using Microsoft.EntityFrameworkCore;

namespace BeeFriends.Context
{
    public class AppDBContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }

        public AppDBContext(DbContextOptions options) : base(options)
        {

        }
    }
}

using BeeFriends.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace BeeFriends.Context
{
    public class AppDBContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Message> Messages { get; set; }

        public AppDBContext(DbContextOptions options) : base(options)
        {

        }
    }
}

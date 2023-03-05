using BeeFriends.Models;
using Microsoft.EntityFrameworkCore;

namespace BeeFriends.Context
{
    public class AppDBContext : DbContext
    {
        public  AppDBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Room> Rooms { get; set; }
        
    }
}

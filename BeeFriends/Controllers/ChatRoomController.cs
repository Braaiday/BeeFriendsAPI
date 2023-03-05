using BeeFriends.Context;
using BeeFriends.Hubs;
using BeeFriends.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BeeFriends.Controllers
{
    [ApiController]
    public class ChatRoomController : ControllerBase
    {
        private readonly AppDBContext _context;
        public ChatRoomController(AppDBContext context)
        {
            _context = context;

        }
        [HttpGet]
        [Route("api/ChatRoom")]
        public async Task<List<Room>> GetAllChatRooms()
        {
            return await _context.Rooms.ToListAsync();
        }
        [HttpPost]
        [Route("api/CreateChatRoom")]
        public async Task<Room> CreateChatRoom(Room room)
        {
            var roomNameTaken = _context.Rooms.Where(r => r.name == room.name).FirstOrDefault();
            if (roomNameTaken != null)
            {
                return null;
            }
            await _context.Rooms.AddAsync(new Room { name = room.name });
            await _context.SaveChangesAsync();
            return await _context.Rooms.FirstOrDefaultAsync(r => r.name == room.name);
        }
    }
}

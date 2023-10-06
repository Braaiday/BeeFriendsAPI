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
            var rooms = _context.Rooms.ToListAsync();
            return await _context.Rooms.ToListAsync();
        }
        [HttpPost]
        [Route("api/CreateChatRoom")]
        public async Task<IActionResult> CreateChatRoom(Room room)
        {
            var roomNameTaken = _context.Rooms.Where(r => r.name == room.name).FirstOrDefault();
            if (roomNameTaken != null)
            {
                return BadRequest("Room already exists.");
            }
            await _context.Rooms.AddAsync(new Room { name = room.name });
            await _context.SaveChangesAsync();
            return Ok(await _context.Rooms.FirstOrDefaultAsync(r => r.name == room.name));
        }
        [HttpPost]
        [Route("api/GetChatHistory")]
        public async Task<List<Message>> GetChatHistory(Room room)
        {
            return await _context.Messages.Where(m => m.RoomId == room.Id).ToListAsync();
        }
    }
}

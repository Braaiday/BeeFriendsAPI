using BeeFriends.Context;
using BeeFriends.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeeFriends.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatRoomController : ControllerBase
    {
        private readonly AppDBContext _context; 
        public ChatRoomController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public List<ChatRoom> GetAllChatRooms()
        {
            return _context.ChatRooms.ToList();
        }

        [HttpPost]
        public IActionResult AddUserToChatRoom()
        {
         
            return Ok();
        }

        [HttpPost]
        public IActionResult SendMessage()
        {

            return Ok();
        }

        [HttpPost]
        public IActionResult CreateChatRoom()
        {
            return Ok();
        }
    }
}

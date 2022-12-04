using BeeFriends.Context;
using BeeFriends.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeeFriends.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDBContext _context;

        public UserController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet()]
        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        [HttpPost]
        public IActionResult SaveUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return Created("api/user" + user.Id, user);
        }
    }
}

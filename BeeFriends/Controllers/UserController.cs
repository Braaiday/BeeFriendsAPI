using BeeFriends.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BeeFriends.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly string _botUser = "MyChat Bot";
        public UserController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpPost]
        [Route("api/SaveUsername")]
        public async Task<IActionResult> SaveUsername([FromBody] string username)
        {
            // Restrict usernames with the same name as the bot
            if (username == _botUser) return BadRequest("Username has been taken.");

            // Check if the cache already contains a list of usernames
            if (!_memoryCache.TryGetValue("UsernamesList", out List<string> cachedUsernames))
            {
                // If not, create a new list
                cachedUsernames = new List<string>();
                // Set it in the cache with an expiration of 24 hours
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24),
                };
                _memoryCache.Set("UsernamesList", cachedUsernames, cacheEntryOptions);
            }

            // Check if the username is already in the list
            if (cachedUsernames.Contains(username))
            {
                return BadRequest("Username has been taken.");
            }

            // Add the new username to the list
            cachedUsernames.Add(username);

            // Return an OK response indicating success
            return Ok(username);
        }
    }
}

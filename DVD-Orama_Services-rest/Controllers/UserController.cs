using DVD_Orama_Services_rest.Data;
using DVD_Orama_Services_rest.Models.DTOs;
using DVD_Orama_Services_rest.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DVD_Orama_Services_rest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/user/check-username?username=xxx
        [HttpGet("check-username")]
        public async Task<IActionResult> CheckUsername([FromQuery] string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return BadRequest("Username is required.");

            var taken = await _context.Users.AnyAsync(u => u.Username == username);
            return taken ? Conflict("Username is already taken.") : Ok("Available.");
        }

        // POST api/user/sync
        [HttpPost("sync")]
        public async Task<IActionResult> Sync([FromBody] SyncUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FirebaseUid) ||
                string.IsNullOrWhiteSpace(dto.Username) ||
                string.IsNullOrWhiteSpace(dto.Email))
                return BadRequest("FirebaseUid, Username and Email are required.");

            var existing = await _context.Users.FirstOrDefaultAsync(u => u.FirebaseUid == dto.FirebaseUid);

            if (existing != null)
            {
                existing.Email = dto.Email;
                existing.Username = dto.Username;
            }
            else
            {
                if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                    return Conflict("Email is already registered.");

                if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                    return Conflict("Username is already taken.");

                _context.Users.Add(new User
                {
                    FirebaseUid = dto.FirebaseUid,
                    Username = dto.Username,
                    Email = dto.Email
                });
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}

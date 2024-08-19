using ClanChat.API.Models.Auth.Requests;
using ClanChat.API.Models.Auth.Responces;
using Microsoft.AspNetCore.Mvc;
using ClanChat.Shared.Database;
using ClanChat.Shared.Database.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace ClanChat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ClanChatDbContext _dbContext;

        public AuthController(ClanChatDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Name == request.Username);

            if (user != null && user.Password == request.Password)
            {
                return Ok(new LoginResponse
                {
                    Token = "dummy-token",
                    UserId = user.Id.ToString(),
                    Username = user.Name
                });
            }

            return Unauthorized();
        }
    }
}

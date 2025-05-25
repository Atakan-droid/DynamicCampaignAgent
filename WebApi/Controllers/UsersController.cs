using Agents.CampaignAgent;
using Agents;
using Agents.UserAgent;
using Data;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserAgent _userAgent;
        private readonly IUserService _userService;
        public UsersController(UserAgent userAgent, IUserService userService)
        {
            _userAgent = userAgent;
            _userService = userService;
        }

        [HttpPost("profile")]
        public async Task<IActionResult> GetProfile([FromBody] ProfileRequest request)
        {
            var result = await _userAgent.RunAsync(request.UserId);
            return Ok(result);
        }
        [HttpGet("users")]
        public async Task<IActionResult> ListUsers()
        {
            var users = await _userService.GetAllUserProfilesAsync();
            return Ok(users);
        }

        [HttpPost("user")]
        public async Task<IActionResult> AddUser([FromBody] UserProfile user)
        {
            var result = await _userService.AddUserAsync(user);
            return CreatedAtAction(nameof(ListUsers), new { id = result?.Id }, result);
        }

        [HttpPost("users/bulk")]
        public async Task<IActionResult> AddUsersBulk([FromBody] List<UserProfile> users)
        {
            var result = await _userService.AddUsersAsync(users);
            return Ok(result);
        }

        [HttpPut("user")]
        public async Task<IActionResult> UpdateUser([FromBody] UserProfile user)
        {
            var result = await _userService.UpdateUserAsync(user);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("user/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}

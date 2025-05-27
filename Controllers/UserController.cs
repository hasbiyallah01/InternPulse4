using InternPulse4.Core.Application.Interfaces.Repositories;
using InternPulse4.Core.Application.Interfaces.Services;
using InternPulse4.Models.UserModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InternPulse4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private ILogger<UserController> _logger;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users.Value);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            var user = await _userService.GetUser(id);
            if (!user.IsSuccessful || user == null)
            {
                _logger.LogError("User not found: {UserId}", id);
                return NotFound(new { Message = user.Message });
            }
            var result = new JsonResult(user.Value)
            {
                StatusCode = (int?)HttpStatusCode.OK
            };
            return result;
        }

        
        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] UserRequest request)
        {
            var userResponse = await _userService.UpdateUser(id, request);
            if (userResponse.IsSuccessful)
            {
                _logger.LogInformation("User updated successfully: {UserId}", id);
                return Ok(new { Message = userResponse.Message });
            }
            _logger.LogError("User update failed: {UserMessage}", userResponse.Message);
            return BadRequest(new { Message = userResponse.Message });
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var userResponse = await _userService.RemoveUser(id);
            if (userResponse.IsSuccessful)
            {
                _logger.LogInformation("User deleted successfully: {UserId}", id);
                return Ok(new { Message = userResponse.Message });
            }
            _logger.LogError("User deletion failed: {UserMessage}", userResponse.Message);
            return BadRequest(new { Message = userResponse.Message });
        }

    }
}

using InternPulse4.Core.Application.Interfaces.Repositories;
using InternPulse4.Core.Application.Interfaces.Services;
using InternPulse4.Core.Application.Services;
using InternPulse4.Core.Domain.Entities;
using InternPulse4.Jwt;
using InternPulse4.Models;
using InternPulse4.Models.UserModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using static InternPulse4.Models.UserModel.UserRequest;

namespace InternPulse4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenGenerator _jwtTokenGenerator;


        public AuthController(IUserService userService, IConfiguration config, IUserRepository userRepository, JwtTokenGenerator jwtTokenGenerator)
        {
            _userService = userService;
            _config = config;
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] UserRequest request)
        {
            var user = await _userService.CreateUser(request);

            if (user.IsSuccessful == true)
            {
                return Ok(new { user.Value, user.Message });
            }
            else
            {
                return StatusCode(400, user.Message);
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest model)
        {
            var user = await _userRepository.GetAsync(model.Email);
            if (user == null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Unauthorized(new { message = "Invalid or expired refresh token." });
            }

            var newAccessToken = _jwtTokenGenerator.GenerateToken(user.Id.ToString(), user.Email, user.Role);
            var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(user);

            return Ok(new
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        [HttpPost("login")]
        public async Task<BaseResponse<LoginResponseModel>> Login(LoginRequestModel model)
        {
            var user = await _userRepository.GetAsync(model.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return new BaseResponse<LoginResponseModel>
                {
                    Message = "Invalid credentials",
                    IsSuccessful = false
                };
            }

            var accessToken = _jwtTokenGenerator.GenerateToken(user.Id.ToString(), user.Email, user.Role, user.RememberMe);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(); 

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(user);

            return new BaseResponse<LoginResponseModel>
            {
                Message = "Login successful",
                IsSuccessful = true,

                Value = new LoginResponseModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    RoleName = user.Role.ToString(),
                    FullName = $"{user.FirstName} {user.LastName}",
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                }
            };
        }
    }
}

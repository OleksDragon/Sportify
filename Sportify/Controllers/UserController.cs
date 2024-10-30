// UserController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sportify.Models;
using Sportify.Services;
using Sportify.Services.Interfaces;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sportify.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            var result = await _userService.Register(user);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] JsonElement body)
        {
            var email = body.GetProperty("email").GetString();
            var password = body.GetProperty("password").GetString();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Будь ласка, введіть електронну пошту та пароль.");
            }

            var result = await _userService.Login(email, password);

            if (!result.IsSuccess)
            {
                return Unauthorized(result.Message);
            }

            return Ok(new
            {
                message = "Користувач успішно авторизований.",
                token = result.Token,
                userId = result.UserId,
                userName = result.UserName
            });
        }


        [Authorize]
        [HttpGet("profile/{id}")]
        public async Task<IActionResult> UserProfile(int id)
        {
            var user = await _userService.GetUserProfile(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var success = await _userService.DeleteUser(id);
            if (!success)
            {
                return NotFound();
            }
            return Ok("Користувача успішно видалено.");
        }

        [Authorize]
        [HttpPut("profile/update/{id}")]
        public async Task<IActionResult> UpdateProfile(int id, [FromBody] User updatedUser)
        {
            var result = await _userService.UpdateUserProfile(id, updatedUser);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }


        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok("Користувач успішно вийшов з аккаунта.");
        }

    }
}

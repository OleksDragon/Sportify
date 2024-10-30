using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sportify.Data;
using Sportify.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Sportify.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly SportifyContext _context;
        private readonly IConfiguration _configuration;

        public UserController(SportifyContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // Регистрация пользователя
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Користувач з такою електронною поштою вже існує!");
                    return BadRequest(ModelState);
                }

                // Хеширование пароля с помощью BCrypt
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Реєстрація успішна!" });
            }

            return BadRequest(ModelState);
        }

        // Вход в систему
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] JsonElement body)
        {
            var email = body.GetProperty("email").GetString();
            var password = body.GetProperty("password").GetString();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Будь ласка, введіть електронну пошту та пароль.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return Unauthorized("Невірний логін або пароль.");
            }

            // Создание JWT токена
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return Ok(new
            {
                message = "Користувач успішно авторизований.",
                token = new JwtSecurityTokenHandler().WriteToken(token),
                userId = user.Id
            });
        }

        // Профиль пользователя
        [Authorize]
        [HttpGet("profile/{id}")]
        public async Task<IActionResult> UserProfile(int id)
        {
            var user = await _context.Users
                .Include(u => u.Workouts)
                .Include(u => u.Progresses)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // Выход из системы
        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok("Користувач успішно вийшов.");
        }

        // Удаление пользователя
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok("Користувача успішно видалено.");
            }

            return NotFound();
        }
    }    
}

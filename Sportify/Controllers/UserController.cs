using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sportify.Data;
using Sportify.Models;
using System.Text.Json;

namespace Sportify.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly SportifyContext _context;

        public UserController(SportifyContext context)
        {
            _context = context;
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
            if (user == null)
            {
                return Unauthorized("Невірний логін або пароль.");
            }

            // Сравнение пароля с хешем
            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return Ok(new { message = "Користувач успішно авторизований.", userId = user.Id });
            }

            return Unauthorized("Невірний логін або пароль.");
        }

        // Профиль пользователя
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
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok("Користувач успішно вийшов.");
        }

        // Удаление пользователя
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

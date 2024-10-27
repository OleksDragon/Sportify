// UserController - для управления пользователями, регистрации, авторизации
using Microsoft.AspNetCore.Mvc;
using Sportify.Data;
using Sportify.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sportify.Controllers
{
    public class UserController : Controller
    {
        private readonly SportifyContext _context;

        public UserController(SportifyContext context)
        {
            _context = context;
        }

        // Регистрация
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                // Проверка уникальности email
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Користувач з такою електронною поштою вже існує!");
                    return View(user);
                }

                // Сохранение нового пользователя
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }

            return View(user);
        }

        // Вход в систему
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Будь ласка, введіть електронну пошту та пароль.");
                return View();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                return RedirectToAction("UserProfile", new { id = user.Id });
            }

            ModelState.AddModelError("", "Невірний логін або пароль.");
            return View();
        }

        // Профиль пользователя
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

            return View(user);
        }

        // Выход из системы
        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }

        // Удаление пользователя
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return NotFound();
        }
    }
}

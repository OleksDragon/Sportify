//UserService - бизнес-логика для управления пользователями (регистрация, вход, профиль).
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sportify.Data;
using Sportify.Models;
using Sportify.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sportify.Services
{
    public class UserService : IUserService
    {
        private readonly SportifyContext _context;
        private readonly IConfiguration _configuration;

        public UserService(SportifyContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<RegistrationResult> Register(User user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                return new RegistrationResult { IsSuccess = false, Message = "Користувач із таким email вже існує!" };
            }

            // Установка роли по умолчанию, если она не указана
            if (string.IsNullOrEmpty(user.Role))
            {
                user.Role = "user";
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new RegistrationResult { IsSuccess = true, Message = "Реєстрація успішна!" };
        }


        public async Task<LoginResult> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return new LoginResult { IsSuccess = false, Message = "Невірний логін або пароль." };
            }

            // Создание JWT токена
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );

            return new LoginResult
            {
                IsSuccess = true,
                UserId = user.Id,
                UserName = user.UserName,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Role = user.Role,
            };
        }

        public async Task<User?> GetUserProfile(int id)
        {
            return await _context.Users
                .Include(u => u.Workouts)
                .Include(u => u.Progresses)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.Workouts)
                .Include(u => u.Progresses)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return false;
            }

            // Удаление связанных данных
            if (user.Workouts != null)
            {
                _context.Workouts.RemoveRange(user.Workouts);
            }

            if (user.Progresses != null)
            {
                _context.Progresses.RemoveRange(user.Progresses);
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<UpdateProfileResult> UpdateUserProfile(int id, User updatedUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return new UpdateProfileResult { IsSuccess = false, Message = "Користувача не знайдено." };
            }

            if (!string.IsNullOrEmpty(updatedUser.UserName))
            {
                user.UserName = updatedUser.UserName;
            }

            if (!string.IsNullOrEmpty(updatedUser.Email))
            {
                user.Email = updatedUser.Email;
            }

            if (!string.IsNullOrEmpty(updatedUser.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(updatedUser.Password);
            }

            if (updatedUser.Goals != null)
            {
                user.Goals = updatedUser.Goals;
            }

            if (!string.IsNullOrEmpty(updatedUser.PhotoBase64))
            {
                user.PhotoBase64 = updatedUser.PhotoBase64;
            }

            if (!string.IsNullOrEmpty(updatedUser.Gender))
            {
                user.Gender = updatedUser.Gender;
            }

            if (updatedUser.Age.HasValue)
            {
                user.Age = updatedUser.Age;
            }

            if (updatedUser.Height.HasValue)
            {
                user.Height = updatedUser.Height;
            }

            if (updatedUser.Weight.HasValue)
            {
                user.Weight = updatedUser.Weight;
            }

            if (!string.IsNullOrEmpty(updatedUser.Role))
            {
                user.Role = updatedUser.Role;
            }

            if (!string.IsNullOrEmpty(updatedUser.TelegramUsername))
            {
                user.TelegramUsername = updatedUser.TelegramUsername;
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new UpdateProfileResult { IsSuccess = true, Message = "Профіль успішно оновлено." };
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<UpdateProfileResult> UpdateUserRole(int id, string role)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return new UpdateProfileResult
                {
                    IsSuccess = false,
                    Message = "Користувача не знайдено."
                };
            }

            if (string.IsNullOrEmpty(role))
            {
                return new UpdateProfileResult
                {
                    IsSuccess = false,
                    Message = "Роль не может быть пустой."
                };
            }

            user.Role = role;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return new UpdateProfileResult
            {
                IsSuccess = true,
                Message = "Роль користувача успішно оновлено."
            };
        }

    }


    public class RegistrationResult
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }

    public class LoginResult
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Token { get; set; }
        public string? Role { get; set; }
    }

    public class UpdateProfileResult
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }


    public class RoleUpdateRequest
    {
        public string Role { get; set; }
    }
}

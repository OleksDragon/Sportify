using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sportify.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ім'я користувача обов'язкове!")]
        [StringLength(100, ErrorMessage = "Ім'я не повинно перевищувати 100 символів.")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Необхідно вказати адресу електронної пошти!")]
        [EmailAddress(ErrorMessage = "Недійсний формат електронної пошти!")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Потрібно ввести пароль!")]
        [MinLength(8, ErrorMessage = "Пароль повинен містити щонайменше 8 символів!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$",
            ErrorMessage = "Пароль повинен містити хоча б одну заголовну літеру, одну малу літеру та одну цифру!")]
        public string Password { get; set; } = null!;

        public string? Goals { get; set; } = null;

        [StringLength(100000, ErrorMessage = "Розмір зображення перевищує допустимий ліміт.")]
        public string? PhotoBase64 { get; set; } = null;

        [RegularExpression("^(Чоловік|Жінка)$", ErrorMessage = "Пол має бути 'Чоловік' або 'Жінка'.")]
        public string? Gender { get; set; } = null;

        [Range(12, 120, ErrorMessage = "Вік має бути від 12 до 120 років.")]
        public int? Age { get; set; } = null;

        [Range(120, 250, ErrorMessage = "Зріст має бути від 120 до 250 см.")]
        public int? Height { get; set; } = null;

        [Range(30, 200, ErrorMessage = "Вага має бути від 30 до 200 кг.")]
        public double? Weight { get; set; } = null;

        [Required]
        [RegularExpression("^(admin|trainer|user)$", ErrorMessage = "Роль має бути 'admin', 'trainer' або 'user'.")]
        public string Role { get; set; } = "user";

        // Добавляем новое поле для Telegram
        [StringLength(100, ErrorMessage = "Ім'я користувача Telegram не повинно перевищувати 100 символів.")]
        public string? TelegramUsername { get; set; } = null;

        [JsonIgnore]
        public virtual ICollection<Workout>? Workouts { get; set; } = null;

        [JsonIgnore]
        public virtual ICollection<Progress>? Progresses { get; set; } = null;
    }
}

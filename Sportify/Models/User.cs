using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sportify.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Необхідно вказати адресу електронної пошти!")]
        [EmailAddress(ErrorMessage = "Недійсний формат електронної пошти!")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Необхідно ввести пароль!")]
        [MinLength(8, ErrorMessage = "Пароль має бути не менше 8 символів!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$", ErrorMessage = "Пароль має містити принаймні одну велику літеру, одну малу літеру та одну цифру!")]
        public required string Password { get; set; }

        public string? Goals { get; set; }

        [StringLength(100000, ErrorMessage = "Розмір зображення перевищує допустимий ліміт.")]
        public string? PhotoBase64 { get; set; }

        //[Required(ErrorMessage = "Необхідно вказати стать!")]
        [RegularExpression("^(Чоловік|Жінка)$", ErrorMessage = "Пол має бути 'Чоловік' або 'Жінка'.")]
        public string? Gender { get; set; }

        [Range(12, 120, ErrorMessage = "Вік має бути від 12 до 120 років.")]
        public int? Age { get; set; }
       
        [Range(120, 250, ErrorMessage = "Зріст має бути в діапазоні від 120 до 250 см.")]
        public int? Height { get; set; }

        [Range(30, 200, ErrorMessage = "Вага має бути в діапазоні від 30 до 200 кг.")]
        public double? Weight { get; set; }

        // Связь с тренировками
        [JsonIgnore]
        public virtual ICollection<Workout>? Workouts { get; set; }

        // Связь с прогрессом
        [JsonIgnore]
        public virtual ICollection<Progress>? Progresses { get; set; }
    }
}

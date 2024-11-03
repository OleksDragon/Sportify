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

        // Связь с тренировками
        [JsonIgnore]
        public virtual ICollection<Workout>? Workouts { get; set; }

        // Связь с прогрессом
        [JsonIgnore]
        public virtual ICollection<Progress>? Progresses { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sportify.Models
{
    public class Exercise //Упражнение
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }  // Название упражнения

        public string? Description { get; set; }  // Описание упражнения

        public int UserId { get; set; }

        // Состояние true обозначает, что только тренер или админ могут менять или удалять упражнение (это свойство не будет меняться)
        public bool Unchanged { get; set; } = false;

        // Связь с пользователем
        [JsonIgnore]
        public virtual User? User { get; set; }

        // Связь с тренировкой
        [JsonIgnore]
        public virtual ICollection<Workout> Workouts { get; set; } = new List<Workout>();
    }
}

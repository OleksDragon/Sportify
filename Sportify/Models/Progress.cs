using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sportify.Models
{
    public class Progress
    {
        public int Id { get; set; }

        [Required]
        public int Weight { get; set; }  // Вес (если применимо)

        [Required]
        public int Reps { get; set; }  // Количество повторений

        [Required]
        public DateTime Date { get; set; }  // Дата записи прогресса

        // Связь с пользователем
        [JsonIgnore]
        public virtual User? User { get; set; }

        // Связь с тренировкой
        [JsonIgnore]
        public virtual Workout? Workout { get; set; }
    }
}

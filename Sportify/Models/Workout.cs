using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sportify.Models
{
    public class Workout // Тренировка
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }  // Название тренировки

        [Required]
        public DateTime Date { get; set; }  // Дата тренировки

        public string? Description { get; set; }  // Описание тренировки

        [Required]
        public int WorkoutTypeId { get; set; }

        public WorkoutType? WorkoutType { get; set; } // Тип тренировки (кардио, силовая)

        [Required]
        public string WorkoutGoal { get; set; } // Цель тренировки (набор массы, похудение)

        [Required]
        [Range(minimum: 1, maximum: 10)]
        public int Complexity { get; set; } // Сложность (от 1 до 10)

        public string Comment { get; set; } = "";

        public bool IsCompleted { get; set; } = false;

        public int UserId { get; set; }

        // Связь с пользователем
        [JsonIgnore]
        public virtual User? User { get; set; }

        // Связь с упражнениями
        [JsonIgnore]
        public virtual ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();

        // Связь с прогрессом
        [JsonIgnore]
        public virtual ICollection<Progress> Progresses { get; set; } = new List<Progress>();
    }
}

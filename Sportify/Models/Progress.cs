using System.ComponentModel.DataAnnotations;

namespace Sportify.Models
{
    public class Progress
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }  // Внешний ключ для связи с пользователем

        [Required]
        public int WorkoutId { get; set; }  // Внешний ключ для связи с тренировкой

        [Required]
        public int Weight { get; set; }  // Вес (если применимо)

        [Required]
        public int Reps { get; set; }  // Количество повторений

        [Required]
        public DateTime Date { get; set; }  // Дата записи прогресса

        // Связь с пользователем
        public virtual User? User { get; set; }

        // Связь с тренировкой
        public virtual Workout? Workout { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Sportify.Models
{
    public class Exercise //Упражнение
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }  // Название упражнения

        public string? Description { get; set; }  // Описание упражнения

        [Required]
        public int WorkoutId { get; set; }  // Внешний ключ для связи с тренировкой

        // Связь с тренировкой
        public virtual Workout? Workout { get; set; }
    }
}

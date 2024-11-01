using System.ComponentModel.DataAnnotations;

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

        // Связь с пользователем
        public virtual User? User { get; set; }

        // Связь с упражнениями
        public virtual ICollection<Exercise> Exercises { get; set; } = new List<Exercise>();

        // Связь с прогрессом
        public virtual ICollection<Progress> Progresses { get; set; } = new List<Progress>();
    }
}

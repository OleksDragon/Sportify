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

        // Связь с тренировкой
        [JsonIgnore]
        public virtual ICollection<Workout> Workouts { get; set; } = new List<Workout>();
    }
}

using Sportify.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sportify.Services
{
    public interface IExerciseService
    {
        Task<IEnumerable<Exercise>> GetAllExercisesAsync(); // Получение списка всех упражнений
        Task<Exercise?> GetExerciseByIdAsync(int id); // Получение упражнения по Id
        Task AddExerciseAsync(Exercise exercise); // Добавление нового упражнения
        Task UpdateExerciseAsync(Exercise exercise); // Обновление существующего упражнения
        Task DeleteExerciseAsync(int id); // Удаление упражнения
    }
}

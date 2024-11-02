using Sportify.Models;

namespace Sportify.Services.Interfaces
{
    public interface IWorkoutService
    {
        // Создание тренировки с привязкой к пользователю
        Task<bool> CreateWorkoutAsync(int id, Workout workout);

        // Получение всех тренировок пользователя
        Task<ICollection<Workout>> GetAllUserWorkouts(int userId);

        // Получение тренировки по Id
        Task<Workout> GetWorkoutById(int id);

        // Обновление тренировки
        Task<bool> UpdateWorkout(int id, Workout newWorkout);

        // Удаление тренировки
        Task<bool> DeleteWorkout(int id);
    }
}

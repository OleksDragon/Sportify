using Sportify.Models;

namespace Sportify.Services.Interfaces
{
    public interface IWorkoutService
    {
        // Создание тренировки с привязкой к пользователю
        Task<bool> CreateWorkoutAsync(int id, Workout workout);

        // Получение всех тренировок пользователя
        Task<ICollection<Workout>> GetAllUserWorkouts(int userId);

        // Получение всех упражнений тренировки
        Task<ICollection<Exercise>> GetExercisesByWorkoutId(int workoutId);

        // Установка упражнений тренировки
        Task<bool> SetExercisesByWorkoutId(int workoutId, ICollection<int> exercisesId);

        // Получение тренировки по Id
        Task<Workout> GetWorkoutById(int id);

        // Обновление тренировки
        Task<bool> UpdateWorkout(int id, Workout newWorkout);

        Task<bool> CompleteWorkout(int id);

        // Удаление тренировки
        Task<bool> DeleteWorkout(int id);
    }
}

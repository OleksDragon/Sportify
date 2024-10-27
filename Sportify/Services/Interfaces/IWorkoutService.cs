using Sportify.Models;

namespace Sportify.Services.Interfaces
{
    public interface IWorkoutService
    {
        // Создание тренировки
        bool CreateWorkout(Workout workout);

        // Получение всех тренировок пользователя
        ICollection<Workout> GetAllUserWorkouts(int userId);

        // Получение тренировки по Id
        Workout GetWorkoutById(int id);

        // Обновление тренировки
        bool UpdateWorkout(int id, Workout newWorkout);

        // Удаление тренировки
        bool DeleteWorkout(int id);
    }
}

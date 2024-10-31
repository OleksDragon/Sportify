using Sportify.Models;

namespace Sportify.Services.Interfaces
{
    public interface IProgressService
    {
        // Создание записи прогресса
        Task<bool> StartProgres(Progress progress);

        // Получение всех прогрессов конкретного пользователя
        Task<ICollection<Progress>> GetAllUserProgresses(int userId);

        // Получение записи по id
        Task<Progress> GetProgressById(int id);

        // Обновление
        Task<bool> UpdateProgress(int id, Progress newProgress);

        // Удаление
        Task<bool> DeleteProgress(int id); 
    }
}

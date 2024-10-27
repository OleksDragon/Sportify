using Sportify.Models;

namespace Sportify.Services.Interfaces
{
    public interface IProgressService
    {
        // Создание записи прогресса
        bool StartProgres(Progress progress);

        // Получение всех прогрессов конкретного пользователя
        ICollection<Progress> GetAllUserProgresses(int userId);

        // Получение записи по id
        Progress GetProgressById(int id);

        // Обновление
        bool UpdateProgress(int id, Progress newProgress);

        // Удаление
        bool DeleteProgress(int id); 
    }
}

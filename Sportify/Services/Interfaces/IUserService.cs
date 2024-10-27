using Sportify.Models;

namespace Sportify.Services.Interfaces
{
    public interface IUserService
    {
        // Создание пользователя
        bool Register(User user);

        // Получение всех пользователей
        ICollection<User> GetAllUsers();

        // Получение пользователя по Id
        User GetUserById(int id);
    }
}

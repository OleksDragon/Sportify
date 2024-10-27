namespace Sportify.Services.Interfaces
{
    public interface INotificationService
    {
        // Отправка напоминания
        void SendNotificaton(string theme, string message);
    }
}

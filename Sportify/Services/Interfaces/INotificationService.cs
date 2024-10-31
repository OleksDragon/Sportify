namespace Sportify.Services.Interfaces
{
    public interface INotificationService
    {
        // Отправка напоминания
        Task SendNotificaton(string toEmail, string theme, string message);
    }
}

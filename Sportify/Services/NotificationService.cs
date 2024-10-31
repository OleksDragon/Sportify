using Sportify.Services.Interfaces;
using System.Net.Mail;
using System.Net;

namespace Sportify.Services
{
    public class NotificationService : INotificationService
    {
        public async Task SendNotificaton(string toEmail, string theme, string message)
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя (адресс будет создан позже)
            MailAddress from = new MailAddress("sportify@gmail.com", "Sportify Team");
            // кому отправляем
            MailAddress to = new MailAddress(toEmail);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = theme;
            // текст письма
            m.Body = $"<h3>{message}</h3>";
            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            // smtp.gmail.com - gmail server
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            // логин и пароль
            // Нужно сменить пароль как только почта быдет создана
            smtp.Credentials = new NetworkCredential("sportify@gmail.com", "mypassword");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
        }
    }
}

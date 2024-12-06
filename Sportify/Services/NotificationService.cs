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
            MailAddress from = new MailAddress("oursportifyteam@gmail.com", "Sportify Team");
            // кому отправляем
            MailAddress to = new MailAddress(toEmail);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = theme;
            // текст письма
            m.Body = message;
            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            // smtp.gmail.com - gmail server
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            // логин и пароль
            smtp.Credentials = new NetworkCredential("oursportifyteam@gmail.com", "mwqm krgg avtb suro");
            smtp.EnableSsl = true;
            try
            {
                await smtp.SendMailAsync(m);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
    }
}

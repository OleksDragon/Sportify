using Microsoft.EntityFrameworkCore;
using Sportify.Data;
using Sportify.Services.Interfaces;

namespace Sportify.BackgroundServices
{
    // Для отправки увидомлений о тренировках в фоновом режиме
    public class SendNotificationService : BackgroundService
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger<SendNotificationService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public SendNotificationService(INotificationService notificationService, ILogger<SendNotificationService> logger, IServiceProvider serviceProvider)
        {
            _notificationService = notificationService;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background service started!");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Без остановки потока
                    Task.Run(() => CheckAndSendNotification());

                    // Каждые 5 минут сервер проверяет у кого до тренировки осталось 55 - 60 минут и отправляет уведомление
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            _logger.LogInformation("Background service stoped!");
        }

        private async Task CheckAndSendNotification()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<SportifyContext>();
                var futureWorkouts = await _context.Workouts.Include(w => w.User).Where(w => w.Date >= DateTime.Now.AddHours(1).AddMinutes(-1) && w.Date < DateTime.Now.AddHours(1)).ToListAsync();

                foreach (var workout in futureWorkouts)
                {
                    var user = workout.User;
                    string emailHtml = @$"
                    <!DOCTYPE html>
                    <html lang='uk'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Нагадування про тренування</title>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                line-height: 1.6;
                                background-color: #f4f4f9;
                                margin: 0;
                                padding: 0;
                            }}
                            .email-container {{
                                max-width: 600px;
                                margin: 20px auto;
                                background: #ffffff;
                                border-radius: 10px;
                                box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
                                overflow: hidden;
                            }}
                            .header {{
                                background-color: #007BFF;
                                color: white;
                                text-align: center;
                                padding: 20px;
                            }}
                            .header h1 {{
                                margin: 0;
                                font-size: 24px;
                            }}
                            .content {{
                                padding: 20px;
                                color: #333;
                            }}
                            .content p {{
                                margin: 10px 0;
                            }}
                            .button {{
                                display: inline-block;
                                padding: 10px 20px;
                                color: white;
                                background-color: #007BFF;
                                text-decoration: none;
                                border-radius: 5px;
                                margin-top: 20px;
                            }}
                            .footer {{
                                text-align: center;
                                font-size: 12px;
                                color: #777;
                                padding: 10px;
                                background: #f4f4f9;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='email-container'>
                            <div class='header'>
                                <h1>Нагадування про тренування</h1>
                            </div>
                            <div class='content'>
                                <p>Вітаємо, {user?.UserName}!</p>
                                <p>Нагадуємо, що ваше тренування заплановане на:</p>
                                <p><strong>{workout.Date.ToShortDateString()} о {workout.Date.ToShortTimeString()}</strong></p>
                            </div>
                            <div class='footer'>
                                <p>Ви отримали це повідомлення, оскільки зареєстровані на нашому сайті.</p>
                                <p>&copy; Sportify. Усі права захищені.</p>
                            </div>
                        </div>
                    </body>
                    </html>";

                    Task.Run(() => _notificationService.SendNotificaton(user.Email, "Нагадування", emailHtml));
                }
            }
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Sportify.Data;
using Sportify.Models;
using Sportify.Services;
using Sportify.Services.Interfaces;
using System;
using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

public class TelegramBotService
{
    private readonly TelegramBotClient _botClient;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly List<long> sendNotifications;

    public TelegramBotService(string botToken, IServiceScopeFactory scopeFactory)
    {
        _botClient = new TelegramBotClient(botToken);
        _scopeFactory = scopeFactory;
        sendNotifications = new List<long>();
    }

    public void Start()
    {
        var cts = new CancellationTokenSource();
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>()
        };

        _botClient.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cts.Token);
    }

    private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == UpdateType.Message && update.Message?.Type == MessageType.Text)
        {
            await ProcessMessageAsync(botClient, update.Message.Chat.Id, update.Message.Text, cancellationToken);
        }
        else if (update.Type == UpdateType.CallbackQuery)
        {
            await HandleCallbackQueryAsync(update.CallbackQuery, botClient, cancellationToken); 
        }
    }

    private async Task ProcessMessageAsync(ITelegramBotClient botClient, long chatId, string messageText, CancellationToken cancellationToken)
    {
        if (messageText.StartsWith("/start"))
        {
            await AuthenticateUserAsync(chatId, botClient, cancellationToken); 
        }
        else if (messageText.Contains('$'))
        {
            await SetWorkoutExercises(messageText, chatId, botClient, cancellationToken);
        }
        else if (messageText.Contains('&'))
        {
            await UpdateWorkout(messageText, chatId, botClient, cancellationToken);
        }
        else if (messageText.Contains('|'))
        {
            await AddWorkout(messageText, chatId, botClient, cancellationToken);
        }
        else
        {
            await SendUnknownCommandMessage(chatId, botClient, cancellationToken);
        }
    }

    // Метод для аутентификации
    private async Task AuthenticateUserAsync(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SportifyContext>();

            // Получаем Telegram-username пользователя
            var telegramUsername = await GetTelegramUsernameAsync(chatId, botClient, cancellationToken);

            // Проверяем, есть ли пользователь с таким TelegramUsername в базе данных
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.TelegramUsername == telegramUsername, cancellationToken);

            Console.WriteLine($"Telegram Username: {telegramUsername}");
            Console.WriteLine($"User found: {user?.TelegramUsername ?? "No user found"}");

            if (user != null)
            {
                // Если пользователь найден в базе данных
                await botClient.SendTextMessageAsync(chatId,
                    "Ви успішно аутентифіковані як " + user.TelegramUsername,
                    cancellationToken: cancellationToken);
                await SendMainMenuAsync(chatId, botClient, cancellationToken);  // Отправляем главное меню
            }
            else
            {
                // Если пользователя нет в базе данных
                var registerButton = new InlineKeyboardMarkup(new[]
                {
                    new[] { InlineKeyboardButton.WithUrl("Зареєструватися на Sportify", "https://www.sportify.com/register") }
                });

                await botClient.SendTextMessageAsync(chatId,
                    "Аутентифікація не вдалася. Будь ласка, зареєструйтесь на нашому сайті.",
                    replyMarkup: registerButton,
                    cancellationToken: cancellationToken);
            }
        }
    }

    private async Task NeedNotifyUserAsync(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        if (sendNotifications.Contains(chatId))
        {
            sendNotifications.Remove(chatId);
            await _botClient.SendTextMessageAsync(chatId, "Ви більше не будете отримувати нагадування щодо тренувань");
        }
        else
        {
            sendNotifications.Add(chatId);
            await _botClient.SendTextMessageAsync(chatId, "Нагадування щодо тренувань увімкнено");
        }
    }

        // Получение Telegram-username пользователя
    private async Task<string> GetTelegramUsernameAsync(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        var user = await botClient.GetChatMemberAsync(chatId, chatId);
        return user.User.Username ?? "Невідомий користувач";
    }

    // Главное меню
    private async Task SendMainMenuAsync(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData("Увімкнути/вимкнути нагадування", "notify") },
            new[] { InlineKeyboardButton.WithCallbackData("Додати тренування", "add_workout") },
            new[] { InlineKeyboardButton.WithCallbackData("Оновити тренування", "update_workout") },
            new[] { InlineKeyboardButton.WithCallbackData("Список тренувань", "list") },
            new[] { InlineKeyboardButton.WithCallbackData("Допомога", "help") }
        });

        await botClient.SendTextMessageAsync(chatId,
            "Ласкаво просимо! Оберіть команду:",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }

    private async Task SendUnknownCommandMessage(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(chatId,
            "Невідома команда. Використовуйте /help для довідки.",
            cancellationToken: cancellationToken);
    }

    // Обработка нажатия кнопок в меню (callback)
    private async Task HandleCallbackQueryAsync(CallbackQuery callbackQuery, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        var chatId = callbackQuery.Message.Chat.Id;
        var data = callbackQuery.Data;

        switch (data)
        {
            case "add_workout":
                // Добавление тренировки
                await SendWorkoutDataFormatMessage(chatId, botClient, cancellationToken);
                break;

            case "update_workout":
                // Список действий для обновления
                await SendActionUpdateChoice(chatId, botClient, cancellationToken);
                break;

            case "update_workout_data":
                // Обновление тренировки
                await SendUpdateDataFormat(chatId, botClient, cancellationToken);
                break;

            case "select_exercises":
                // Назначение упражнений
                await SendChangeExercisesFormat(chatId, botClient, cancellationToken);
                break;

            case "list":
                // Список тренировок
                await SendWorkoutTypesMenu(chatId, botClient, cancellationToken);
                break;

            case "help":
                // Помощь
                await SendHelpMessage(chatId, botClient, cancellationToken);
                break;

            case "all_workouts":
                // Отображаем все тренировки
                await ShowAllWorkouts(chatId, botClient, cancellationToken);
                break;

            case "completed_workouts":
                // Отображаем завершенные тренировки
                await ShowCompletedWorkouts(chatId, botClient, cancellationToken);
                break;

            case "upcoming_workouts":
                // Отображаем запланированные тренировки
                await ShowUpcomingWorkouts(chatId, botClient, cancellationToken);
                break;

            case "back_to_menu":
                // Возвращаемся в главное меню
                await SendMainMenuAsync(chatId, botClient, cancellationToken);
                break;

            case "notify":
                // Включаем/выключаем напоминание
                await NeedNotifyUserAsync(chatId, botClient, cancellationToken);
                break;

            default:
                await SendUnknownCommandMessage(chatId, botClient, cancellationToken);
                break;
        }
    }   

    private async Task SendWorkoutDataFormatMessage(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(chatId,
            "Введіть дані про тренування у форматі:\n" +
            "Назва | Дата (YYYY-MM-DD HH:mm) | Тип (1 – кардіо, 2 – силове) | Ціль (1 - схуднення, 2 - набір маси) | Складність (1-10)",
            parseMode: ParseMode.Markdown,
            cancellationToken: cancellationToken);
    }

    private async Task SendActionUpdateChoice(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData("Оновити дані про тренування", "update_workout_data") },
            new[] { InlineKeyboardButton.WithCallbackData("Вибрати вправи", "select_exercises") },
            new[] { InlineKeyboardButton.WithCallbackData("Назад в меню", "back_to_menu") }
        });

        await botClient.SendTextMessageAsync(chatId,
            "Оберіть дію:",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }

    private async Task SendUpdateDataFormat(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        await ShowAllWorkouts(chatId, botClient, cancellationToken);

        await botClient.SendTextMessageAsync(chatId,
            "Введіть дані про тренування у форматі:\n" +
            "Id & Назва | Дата (YYYY-MM-DD HH:mm) | Тип (1 – кардіо, 2 – силове) | Ціль (1 - схуднення, 2 - набір маси) | Складність (1-10)",
            parseMode: ParseMode.Markdown,
            cancellationToken: cancellationToken);
    }

    private async Task SendChangeExercisesFormat(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        await ShowAllWorkouts(chatId, botClient, cancellationToken);
        await ShowAllUserExercises(chatId, botClient, cancellationToken);

        await botClient.SendTextMessageAsync(chatId,
            "Оберіть вправи для тренування у форматі:\n" +
            "Id (тренування) $ Id1 (вправи) | Id2 (вправи) | Id3 (вправи) ... (можна обрати довільну кількість вправ)",
            parseMode: ParseMode.Markdown,
            cancellationToken: cancellationToken);
    }

    private async Task AddWorkout(string messageText, long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        var parts = messageText.Split('|');
        if (parts.Length == 5)
        {
            try
            {
                // Отримуємо Telegram username користувача за chatId
                var telegramUsername = await GetTelegramUsernameAsync(chatId, botClient, cancellationToken);

                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<SportifyContext>();

                    // Знаходимо користувача за його Telegram username
                    var user = await dbContext.Users
                        .FirstOrDefaultAsync(u => u.TelegramUsername == telegramUsername, cancellationToken);

                    if (user == null)
                    {
                        await botClient.SendTextMessageAsync(chatId,
                            "Ви не зареєстровані в системі. Спочатку зареєструйтесь.",
                            cancellationToken: cancellationToken);
                        return;
                    }

                    // Створюємо тренування
                    var workout = new Workout
                    {
                        Name = parts[0].Trim(),
                        Date = DateTime.ParseExact(parts[1].Trim(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                        WorkoutTypeId = int.Parse(parts[2].Trim()),
                        WorkoutGoal = int.Parse(parts[3].Trim()) == 1 ? "Схуднення" : "Набір",
                        Complexity = int.Parse(parts[4].Trim()),
                        IsCompleted = false,
                        User = user
                    };

                    dbContext.Workouts.Add(workout);
                    await dbContext.SaveChangesAsync(cancellationToken);

                    await botClient.SendTextMessageAsync(chatId,
                        $"Тренування '{workout.Name}' успішно додано для користувача {user.UserName}!",
                        cancellationToken: cancellationToken);
                }
            }
            catch
            {
                await botClient.SendTextMessageAsync(chatId,
                    "Помилка у форматі даних. Спробуйте знову.",
                    cancellationToken: cancellationToken);
            }
        }
        else
        {
            await botClient.SendTextMessageAsync(chatId,
                "Невірний формат. Спробуйте ще раз, використовуючи формат: Id & Назва | Дата (YYYY-MM-DD HH:mm) | Тип (1 – кардіо, 2 – силове) | Ціль (1 - схуднення, 2 - набір маси) | Складність (1-10)",
                cancellationToken: cancellationToken);
        }
    }

    private async Task UpdateWorkout(string messageText, long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        int workoutId;
        var parts = messageText.Substring(messageText.IndexOf('&') + 1).Split('|');
        if (parts.Length == 5 && Int32.TryParse(messageText.Substring(0, messageText.IndexOf('&')), out workoutId))
        {
            try
            {
                var telegramUsername = await GetTelegramUsernameAsync(chatId, botClient, cancellationToken);

                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<SportifyContext>();

                    var workout = dbContext.Workouts.FirstOrDefault(w => w.Id == workoutId);

                    if (workout == null)
                    {
                        await botClient.SendTextMessageAsync(chatId,
                        "Тренування з таким id не існує.",
                        cancellationToken: cancellationToken);
                        return;
                    }

                    // Плучаем пользователя по username 
                    var user = await dbContext.Users.Include(u => u.Workouts)
                        .FirstOrDefaultAsync(u => u.TelegramUsername == telegramUsername, cancellationToken);

                    if (user == null)
                    {
                        await botClient.SendTextMessageAsync(chatId,
                            "Ви не зареєстровані в системі. Спочатку зареєструйтесь.",
                            cancellationToken: cancellationToken);
                        return;
                    }

                    // Проверка на то, пренадлежит ли пользователю тренировка
                    if(!user.Workouts.Any(w => w.Id == workout.Id))
                    {
                        await botClient.SendTextMessageAsync(chatId,
                           "Це тренування вам не належить.",
                           cancellationToken: cancellationToken);
                        return;
                    }

                    // Обновляем тренировку
                    workout.Name = parts[0].Trim();
                    workout.Date = DateTime.ParseExact(parts[1].Trim(), "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                    workout.WorkoutTypeId = int.Parse(parts[2].Trim());
                    workout.WorkoutGoal = int.Parse(parts[3].Trim()) == 1 ? "Схуднення" : "Набір";
                    workout.Complexity = int.Parse(parts[4].Trim());

                    dbContext.Workouts.Update(workout);
                    await dbContext.SaveChangesAsync(cancellationToken);

                    await botClient.SendTextMessageAsync(chatId,
                        $"Тренування '{workout.Name}' успішно оновлено!",
                        cancellationToken: cancellationToken);
                }
            }
            catch
            {
                await botClient.SendTextMessageAsync(chatId,
                    "Помилка у форматі даних. Спробуйте знову.",
                    cancellationToken: cancellationToken);
            }
        }
        else
        {
            await botClient.SendTextMessageAsync(chatId,
                "Невірний формат. Спробуйте ще раз, використовуючи формат: Назва | Дата (YYYY-MM-DD HH:mm) | Тип (1 - кардіо, 2 - силове) | Ціль | Складність (1-10)",
                cancellationToken: cancellationToken);
        }
    }

    private async Task SetWorkoutExercises(string messageText, long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        int workoutId;
        var ids = messageText.Substring(messageText.IndexOf('$') + 1).Split('|');
        if (Int32.TryParse(messageText.Substring(0, messageText.IndexOf('$')), out workoutId))
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var workoutService = scope.ServiceProvider.GetRequiredService<IWorkoutService>();

                var dbContext = scope.ServiceProvider.GetRequiredService<SportifyContext>();
                var telegramUsername = await GetTelegramUsernameAsync(chatId, botClient, cancellationToken);

                // Плучаем пользователя по username 
                var user = await dbContext.Users.Include(u => u.Workouts)
                    .FirstOrDefaultAsync(u => u.TelegramUsername == telegramUsername, cancellationToken);

                if (user == null)
                {
                    await botClient.SendTextMessageAsync(chatId,
                        "Ви не зареєстровані в системі. Спочатку зареєструйтесь.",
                        cancellationToken: cancellationToken);
                    return;
                }

                List<int> idsToInt = new List<int>();
                int id;
                foreach (string str in ids)
                {
                    if (int.TryParse(str, out id) && dbContext.Exercises.FirstOrDefault(e => e.Id == id)?.UserId == user.Id)
                    {
                        idsToInt.Add(id);
                    }
                }

                await workoutService.SetExercisesByWorkoutId(workoutId, idsToInt);
            }
            await botClient.SendTextMessageAsync(chatId,
               "Тренування оновлено",
               cancellationToken: cancellationToken);
        }
        else
        {
            await botClient.SendTextMessageAsync(chatId,
               "Невірний формат, спробуйте знову:\n" +
            "Id (тренування) $ Id1 (вправи) | Id2 (вправи) | Id3 (вправи) ... (можна обрати довільну кількість вправ)",
               cancellationToken: cancellationToken);
        }
    }

    private async Task SendWorkoutTypesMenu(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        var inlineKeyboard = new InlineKeyboardMarkup(new[]
        {
            new[] { InlineKeyboardButton.WithCallbackData("Всі тренування", "all_workouts") },
            new[] { InlineKeyboardButton.WithCallbackData("Завершені", "completed_workouts") },
            new[] { InlineKeyboardButton.WithCallbackData("Заплановані", "upcoming_workouts") },
            new[] { InlineKeyboardButton.WithCallbackData("Назад в меню", "back_to_menu") }
        });

        await botClient.SendTextMessageAsync(chatId,
            "Оберіть тип тренувань для відображення:",
            replyMarkup: inlineKeyboard,
            cancellationToken: cancellationToken);
    }

    // Метод для отображения всех тренировок
    private async Task ShowAllWorkouts(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SportifyContext>();
            // Отримуємо Telegram username користувача за chatId
            var telegramUsername = await GetTelegramUsernameAsync(chatId, botClient, cancellationToken);

            // Знаходимо користувача за його Telegram username
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.TelegramUsername == telegramUsername, cancellationToken);

            if (user != null)
            {
                // Получаем тренировки для пользователя
                var workouts = await dbContext.Workouts
                    .Where(w => w.User == user) 
                    .ToListAsync(cancellationToken);

                if (workouts.Any())
                {
                    var workoutsList = string.Join("\n", workouts.Select(w =>
                               $"{w.Id}: {w.Name} - {w.Date:dd.MM.yyyy HH:mm} (Ціль: {w.WorkoutGoal}, Складність: {w.Complexity})"));
                    await botClient.SendTextMessageAsync(chatId,
                        $"Ось ваші тренування:\n{workoutsList}",
                        cancellationToken: cancellationToken);
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId,
                        "У вас ще немає тренувань.",
                        cancellationToken: cancellationToken);
                }
            }
            else
            {
                await botClient.SendTextMessageAsync(chatId,
                    "Не знайдено користувача з таким іменем Telegram.",
                    cancellationToken: cancellationToken);
            }
        }
    }

    // Метод для отображения завершенных тренировок
    private async Task ShowCompletedWorkouts(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SportifyContext>();

            // Отримуємо Telegram username користувача за chatId
            var telegramUsername = await GetTelegramUsernameAsync(chatId, botClient, cancellationToken);

            // Знаходимо користувача за його Telegram username
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.TelegramUsername == telegramUsername, cancellationToken);

            if (user != null)
            {

                // Получаем завершенные тренировки для пользователя
                var workouts = await dbContext.Workouts
                    .Where(w => w.User == user && w.IsCompleted)
                    .ToListAsync(cancellationToken);

                if (workouts.Any())
            {
                var workoutsList = string.Join("\n", workouts.Select(w =>
                               $"{w.Id}: {w.Name} - {w.Date:dd.MM.yyyy HH:mm} (Ціль: {w.WorkoutGoal}, Складність: {w.Complexity})"));
                await botClient.SendTextMessageAsync(chatId,
                    $"Ось ваші завершені тренування:\n{workoutsList}",
                    cancellationToken: cancellationToken);
            }
            else
            {
                await botClient.SendTextMessageAsync(chatId,
                    "У вас немає завершених тренувань.",
                    cancellationToken: cancellationToken);
            }
            }
            else
            {
                await botClient.SendTextMessageAsync(chatId,
                    "Не знайдено користувача з таким іменем Telegram.",
                    cancellationToken: cancellationToken);
            }
        }
    }

    // Метод для отображения запланированных тренировок
    private async Task ShowUpcomingWorkouts(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SportifyContext>();

            // Отримуємо Telegram username користувача за chatId
            var telegramUsername = await GetTelegramUsernameAsync(chatId, botClient, cancellationToken);

            // Знаходимо користувача за його Telegram username
            var user = await dbContext.Users
                .FirstOrDefaultAsync(u => u.TelegramUsername == telegramUsername, cancellationToken);

            if (user != null)
            {
                // Получаем запланированные тренировки для пользователя (по дате) и фильтруем по UserId
                var workouts = await dbContext.Workouts
                    .Where(w => w.User == user && !w.IsCompleted && w.Date > DateTime.Now)
                    .ToListAsync(cancellationToken);

                if (workouts.Any())
                {
                    var workoutsList = string.Join("\n", workouts.Select(w =>
                        $"{w.Id}: {w.Name} - {w.Date:dd.MM.yyyy HH:mm} (Ціль: {w.WorkoutGoal}, Складність: {w.Complexity})"));
                    await botClient.SendTextMessageAsync(chatId,
                        $"Ось ваші заплановані тренування:\n{workoutsList}",
                        cancellationToken: cancellationToken);
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId,
                        "У вас немає запланованих тренувань.",
                        cancellationToken: cancellationToken);
                }
            }
            else
            {
                await botClient.SendTextMessageAsync(chatId,
                    "Не знайдено користувача з таким іменем Telegram.",
                    cancellationToken: cancellationToken);
            }
        }
    }

    // Отображение упражнений 
    private async Task ShowAllUserExercises(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<SportifyContext>();
            var telegramUsername = await GetTelegramUsernameAsync(chatId, botClient, cancellationToken);

            // Плучаем пользователя по username 
            var user = await dbContext.Users.Include(u => u.Workouts)
                .FirstOrDefaultAsync(u => u.TelegramUsername == telegramUsername, cancellationToken);

            if (user == null)
            {
                await botClient.SendTextMessageAsync(chatId,
                    "Ви не зареєстровані в системі. Спочатку зареєструйтесь.",
                    cancellationToken: cancellationToken);
                return;
            }

            var exercises = dbContext.Exercises.Where(e => e.Unchanged || e.UserId == user.Id);
                if (exercises.Any())
                {
                    var exercisesStr = string.Join("\n", exercises.Select(e =>
                               $"{e.Id}: {e.Name} - {e.Description}"));
                    await botClient.SendTextMessageAsync(chatId,
                        $"Вправи:\n{exercisesStr}",
                        cancellationToken: cancellationToken);
                }
                else
                {
                    await botClient.SendTextMessageAsync(chatId,
                        "Вправи не знайдено.",
                        cancellationToken: cancellationToken);
                }
            
        }
    }

    private async Task SendHelpMessage(long chatId, ITelegramBotClient botClient, CancellationToken cancellationToken)
    {
        await botClient.SendTextMessageAsync(chatId,
                          "Ось доступні команди для бота:\n" +
                           "/start - Почати роботу з ботом\n" +
                          "Додати тренування - Додайте тренування, використовуючи формат: Назва | Дата (YYYY-MM-DD HH:mm) | Тип (1 - кардіо, 2 - силове) | Ціль | Складність (1-10)\n" +
                          "Список тренувань - Переглянути список своїх тренувань\n" +
                          "Допомога - Отримати цю інформацію знову",
            cancellationToken: cancellationToken);
    }

    private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Помилка: {exception.Message}");
        return Task.CompletedTask;
    }

    public async Task SendNotify(string username, string time)
    {
        foreach (var id in sendNotifications)
        {
            var chat = await _botClient.GetChatAsync(id);
            if (chat.Username == username)
            {
                await _botClient.SendTextMessageAsync(chat.Id, $"У вас заплановано тренування о {time}");
                break;
            }
        }
    }
}

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sportify.Data;
using Sportify.Services.Interfaces;
using Sportify.Services;
using System.Text;
using System.Security.Claims;
using Sportify.BackgroundServices;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            RoleClaimType = ClaimTypes.Role,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
        };
    });

// Добавление службы базы данных
builder.Services.AddDbContext<SportifyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация IConfiguration
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Регистрация Service
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IWorkoutService, WorkoutService>();
builder.Services.AddScoped<IProgressService, ProgressService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();

// Регистрация TelegramService
var telegramToken = builder.Configuration.GetSection("TelegramSettings")["BotToken"];
builder.Services.AddSingleton(new TelegramBotService(
    telegramToken,
    builder.Services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>()));


// Регистрация фоновых задач
builder.Services.AddHostedService<SendNotificationService>();


// Настройка CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Добавление контроллеров
builder.Services.AddControllers();

var app = builder.Build();

app.Lifetime.ApplicationStarted.Register(() =>
{
    var botService = app.Services.GetRequiredService<TelegramBotService>();
    botService.Start();
});

// Обработка ошибок
app.UseExceptionHandler("/error");

// Применение CORS
app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

// Определение маршрутов
app.MapGet("/", () => "Hello World!");
app.MapGet("/user/{id}", (int id) => $"User ID: {id}");

// Регистрация контроллеров
app.MapControllers();

app.Run();

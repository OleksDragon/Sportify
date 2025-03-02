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

// ���������� ������ ���� ������
builder.Services.AddDbContext<SportifyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ����������� IConfiguration
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// ����������� Service
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IWorkoutService, WorkoutService>();
builder.Services.AddScoped<IProgressService, ProgressService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();

// ����������� TelegramService
var telegramToken = builder.Configuration.GetSection("TelegramSettings")["BotToken"];
builder.Services.AddSingleton(new TelegramBotService(
    telegramToken,
    builder.Services.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>()));


// ����������� ������� �����
builder.Services.AddHostedService<SendNotificationService>();


// ��������� CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ���������� ������������
builder.Services.AddControllers();

var app = builder.Build();

app.Lifetime.ApplicationStarted.Register(() =>
{
    var botService = app.Services.GetRequiredService<TelegramBotService>();
    botService.Start();
});

// ��������� ������
app.UseExceptionHandler("/error");

// ���������� CORS
app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

// ����������� ���������
app.MapGet("/", () => "Hello World!");
app.MapGet("/user/{id}", (int id) => $"User ID: {id}");

// ����������� ������������
app.MapControllers();

app.Run();

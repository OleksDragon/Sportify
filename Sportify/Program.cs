using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sportify.Data;

var builder = WebApplication.CreateBuilder(args);

// Добавление службы базы данных
builder.Services.AddDbContext<SportifyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Добавление контроллеров
builder.Services.AddControllers();

var app = builder.Build();

// Обработка ошибок
app.UseExceptionHandler("/error");

// Определение маршрутов
app.MapGet("/", () => "Hello World!");
app.MapGet("/user/{id}", (int id) => $"User ID: {id}");

// Регистрация контроллеров
app.MapControllers();

app.Run();

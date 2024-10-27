using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sportify.Data;

var builder = WebApplication.CreateBuilder(args);

// ���������� ������ ���� ������
builder.Services.AddDbContext<SportifyContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ���������� ������������
builder.Services.AddControllers();

var app = builder.Build();

// ��������� ������
app.UseExceptionHandler("/error");

// ����������� ���������
app.MapGet("/", () => "Hello World!");
app.MapGet("/user/{id}", (int id) => $"User ID: {id}");

// ����������� ������������
app.MapControllers();

app.Run();

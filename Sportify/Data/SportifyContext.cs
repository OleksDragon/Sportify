using System;
using System.ComponentModel.DataAnnotations;
using Sportify.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Sportify.Data
{
    public class SportifyContext : DbContext
    {
        public SportifyContext(DbContextOptions<SportifyContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutType> WorkoutTypes { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Progress> Progresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Добавляем начальные данные в типы тренировок
            modelBuilder.Entity<WorkoutType>().HasData(
                new WorkoutType { Id = 1, Title="Кардіо", Description= "Вправи, що підвищують пульс і покращують роботу серця та легенів. Приклади: біг, ходьба, плавання, велоспорт, стрибки на скакалці. Вони допомагають спалювати калорії, підвищують витривалість і зміцнюють серцево-судинну систему.", ImageBase64="Згодом!!!!" },
                new WorkoutType { Id = 2, Title = "Силове", Description = "Тренування, спрямовані на зміцнення м''язів та підвищення їх сили. Основні види: вправи з вагою тіла (віджимання, присідання), заняття з гантелями, штангою або на тренажерах. Силові вправи допомагають наростити м''язову масу, покращують обмін речовин та підвищують загальну фізичну витривалість.", ImageBase64 = "Згодом!!!!" }
            );
        }
    }
}

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
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Progress> Progresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка связи между User и Workouts
            modelBuilder.Entity<User>()
                .HasMany(u => u.Workouts)
                .WithOne(w => w.User)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Настройка связи между Workout и Exercises
            modelBuilder.Entity<Workout>()
                .HasMany(w => w.Exercises)
                .WithOne(e => e.Workout)
                .HasForeignKey(e => e.WorkoutId)
                .OnDelete(DeleteBehavior.NoAction);

            // Настройка связи между User и Progresses
            modelBuilder.Entity<User>()
                .HasMany(u => u.Progresses)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Настройка связи между Workout и Progresses
            modelBuilder.Entity<Workout>()
                .HasMany(w => w.Progresses)
                .WithOne(p => p.Workout)
                .HasForeignKey(p => p.WorkoutId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

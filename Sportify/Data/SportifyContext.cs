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
    }
}

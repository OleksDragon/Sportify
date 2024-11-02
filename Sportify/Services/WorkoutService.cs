// WorkoutService - логика работы с тренировками и планами.
using Microsoft.EntityFrameworkCore;
using Sportify.Data;
using Sportify.Models;

// WorkoutService - логика работы с тренировками и планами.
using Sportify.Services.Interfaces;

namespace Sportify.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly SportifyContext _context;
        public WorkoutService(SportifyContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateWorkoutAsync(int id, Workout workout)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    workout.User = user;
                    await _context.Workouts.AddAsync(workout);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DeleteWorkout(int id)
        {
            try
            {
                var workout = await _context.Workouts.FindAsync(id);
                if (workout != null)
                {
                    _context.Workouts.Remove(workout);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public async Task<ICollection<Workout>> GetAllUserWorkouts(int userId)
        {
            return await _context.Workouts.ToArrayAsync();
        }

        public async Task<Workout> GetWorkoutById(int id)
        {
            var workout = await _context.Workouts.FirstOrDefaultAsync(x => x.Id == id);
            return workout; // Может быть null
        }

        public async Task<bool> UpdateWorkout(int id, Workout newWorkout)
        {
            try
            {
                var workout = await _context.Workouts.FirstOrDefaultAsync(x => x.Id == id);

                if (workout != null)
                {
                    if (!string.IsNullOrWhiteSpace(newWorkout.Name))
                    {
                        workout.Name = newWorkout.Name;
                    }

                    if (!string.IsNullOrWhiteSpace(newWorkout.Description))
                    {
                        workout.Description = newWorkout.Description;
                    }

                    if (newWorkout.Date != null)
                    {
                        workout.Date = newWorkout.Date;
                    }

                    _context.Workouts.Update(workout);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
    }
}

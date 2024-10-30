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

        public async Task<bool> CreateWorkoutAsync(Workout workout)
        {
            try
            {
                await _context.Workouts.AddAsync(workout);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteWorkout(int id)
        {
            try
            {
                var workout = await _context.Workouts.FindAsync(id);
                if (workout != null)
                {
                    _context.Workouts.Remove(workout);
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
                    _context.Workouts.Update(workout);
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

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
                var type = _context.WorkoutTypes.FirstOrDefault(t => t.Id == workout.WorkoutTypeId);
                var user = await _context.Users.FindAsync(id);

                if (user != null && type != null)
                {
                    workout.User = user;
                    workout.WorkoutType = type;
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
            return await _context.Workouts.Where(w => w.User.Id == userId).ToArrayAsync();
        }

        public async Task<ICollection<Exercise>> GetExercisesByWorkoutId(int workoutId)
        {
            return await _context.Workouts.Where(w => w.Id == workoutId).SelectMany(w => w.Exercises).ToListAsync();
        }

        public async Task<Workout> GetWorkoutById(int id)
        {
            var workout = await _context.Workouts.FirstOrDefaultAsync(x => x.Id == id);
            return workout; // Может быть null
        }

        public async Task<bool> SetExercisesByWorkoutId(int workoutId, ICollection<int> exercisesId)
        {
            try
            {
                Workout workout = await _context.Workouts
                .Include(w => w.Exercises)
                .FirstOrDefaultAsync(w => w.Id == workoutId);

                if (workout == null)
                {
                    Console.WriteLine("Тренировка не найдена");
                    return false;
                }

                workout.Exercises.Clear();

                foreach (var exerciseId in exercisesId)
                {
                    var exercise = await _context.Exercises.FirstOrDefaultAsync(e => e.Id == exerciseId);

                    if (exercise != null)
                    {
                        workout.Exercises.Add(exercise);
                    }
                    else
                    {
                        Console.WriteLine($"Упражнение не найдено");
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
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

                    var type = _context.WorkoutTypes.FirstOrDefault(x => x.Id == newWorkout.WorkoutTypeId);

                    if (type != null)
                    {
                        workout.WorkoutType = type;
                    }

                    if(newWorkout.Complexity <= 10 && newWorkout.Complexity >= 1)
                    {
                        workout.Complexity = newWorkout.Complexity;
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

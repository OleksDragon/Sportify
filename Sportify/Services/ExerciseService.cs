using Sportify.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sportify.Data;

namespace Sportify.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly SportifyContext _context;

        public ExerciseService(SportifyContext context)
        {
            _context = context;
        }

        // Получение всех упражнений
        public async Task<IEnumerable<Exercise>> GetAllExercisesAsync()
        {
            return await _context.Exercises.Include(e => e.Workouts).ToListAsync();
        }

        // Получение упражнения по ID
        public async Task<Exercise?> GetExerciseByIdAsync(int id)
        {
            var exercise = await _context.Exercises.Include(e => e.Workouts).FirstOrDefaultAsync(e => e.Id == id);
            if (exercise == null)
            {
                throw new KeyNotFoundException($"Упражнение с идентификатором {id} не найдено.");
            }
            return exercise;
        }


        // Добавление нового упражнения
        public async Task AddExerciseAsync(Exercise exercise)
        {
            await _context.Exercises.AddAsync(exercise);
            await _context.SaveChangesAsync();
        }

        // Обновление упражнения
        public async Task UpdateExerciseAsync(Exercise exercise)
        {
            _context.Exercises.Update(exercise);
            await _context.SaveChangesAsync();
        }

        // Удаление упражнения
        public async Task DeleteExerciseAsync(int id)
        {
            var exercise = await GetExerciseByIdAsync(id);
            if (exercise != null)
            {
                _context.Exercises.Remove(exercise);
                await _context.SaveChangesAsync();
            }
        }
    }
}

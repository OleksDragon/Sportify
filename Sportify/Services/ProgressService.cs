// ProgressService - логика обработки прогресса и аналитики.
using Microsoft.EntityFrameworkCore;
using Sportify.Data;
using Sportify.Models;

// ProgressService - логика обработки прогресса и аналитики.
using Sportify.Services.Interfaces;
using System;

namespace Sportify.Services
{
    public class ProgressService : IProgressService
    {
        private readonly SportifyContext _context;
        public ProgressService(SportifyContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteProgress(int id)
        {
            try
            {
                var progress = await _context.Progresses.FindAsync(id);
                if (progress != null)
                {
                    _context.Progresses.Remove(progress);
                    await _context.SaveChangesAsync();
                }
                else return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public async Task<ICollection<Progress>> GetAllUserProgresses(int userId)
        {
            return await _context.Progresses.ToArrayAsync();
        }

        public async Task<Progress> GetProgressById(int id)
        {
            var progress = await _context.Progresses.FindAsync(id);
            return progress; // Может быть null
        }

        public async Task<bool> StartProgress(Progress progress)
        {
            try
            {
                await _context.Progresses.AddAsync(progress);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateProgress(int id, Progress newProgress)
        {
            try
            {
                var progress = await _context.Progresses.FindAsync(id);

                if (progress != null)
                {
                    if (newProgress.Weight >= 0)
                    {
                        progress.Weight = newProgress.Weight;
                    }

                    if (newProgress.Reps != 0)
                    {
                        progress.Reps = newProgress.Reps;
                    }

                    progress.Date = newProgress.Date;
                    _context.Progresses.Update(progress);
                    await _context.SaveChangesAsync();
                }
                else return false;
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

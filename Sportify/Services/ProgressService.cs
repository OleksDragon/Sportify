// ProgressService - логика обработки прогресса и аналитики.
using Sportify.Data;
using Sportify.Models;

// ProgressService - логика обработки прогресса и аналитики.
using Sportify.Services.Interfaces;

namespace Sportify.Services
{
    public class ProgressService : IProgressService
    {
        private readonly SportifyContext _context;
        public ProgressService(SportifyContext context)
        {
            _context = context;
        }
        public bool DeleteProgress(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Progress> GetAllUserProgresses(int userId)
        {
            throw new NotImplementedException();
        }

        public Progress GetProgressById(int id)
        {
            throw new NotImplementedException();
        }

        public bool StartProgres(Progress progress)
        {
            throw new NotImplementedException();
        }

        public bool UpdateProgress(int id, Progress newProgress)
        {
            throw new NotImplementedException();
        }
    }
}

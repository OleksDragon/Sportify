// WorkoutSessionController - для ведения и завершения тренировок.
using Microsoft.AspNetCore.Mvc;

namespace Sportify.Controllers
{
    public class WorkoutSessionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

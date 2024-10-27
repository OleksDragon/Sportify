//WorkoutPlanController - Для создания и редактирования тренировочных планов
using Microsoft.AspNetCore.Mvc;

namespace Sportify.Controllers
{
    public class WorkoutPlanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

// ExerciseController - для добавления и редактирования упражнений.
using Microsoft.AspNetCore.Mvc;

namespace Sportify.Controllers
{
    public class ExerciseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

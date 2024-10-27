// ProgressController - для записи и получения данных о прогрессе.
using Microsoft.AspNetCore.Mvc;

namespace Sportify.Controllers
{
    public class ProgressController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

// NotificationController - для отправки уведомлений и напоминаний.
using Microsoft.AspNetCore.Mvc;

namespace Sportify.Controllers
{
    public class NotificationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

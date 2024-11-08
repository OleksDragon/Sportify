// NotificationController - для отправки уведомлений и напоминаний.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sportify.Services.Interfaces;

namespace Sportify.Controllers
{
    //[ApiController]
    //[Route("api/[controller]")]

    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;

        public NotificationController(INotificationService notificationService, IUserService userService)
        {
            _notificationService = notificationService;
            _userService = userService; 
        }

        // Может быть будет не api
        //[Authorize]
        //[HttpPost("{id}")]
        public async Task<IActionResult> IndexAsync(int userId, AdditionalNotificationClass notification)
        {
            var userEmail = (await _userService.GetUserProfile(userId))?.Email;

            if (userEmail != null)
            {
                await _notificationService.SendNotificaton(userEmail, notification.Theme, notification.Message);

                return Ok();
            }

            return BadRequest();
        }
    }

    public class AdditionalNotificationClass
    {
        public string Theme { get; set; } = String.Empty;
        public string Message { get; set; } = String.Empty;
    }
}

using Application.DTOs.Request.Account;
using Application.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController(INotificationRepository _notificationRepository) : Controller
    {

        [HttpGet("get-notifications")]
        public async Task<ActionResult<List<NotificationDTO>>> GetAllNotifications(string userId)
        {
            try
            {
                var data = await _notificationRepository.GetAllNotifications(userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost("low-qyt-notify")]
        public async Task<IActionResult> LowQuantityNotify(NotificationDTO model)
        {
            try
            {
                var response = await _notificationRepository.LowQuantityNotifyAsync(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("order-placed-notify")]
        public async Task<IActionResult> OrderPlacedNotify(NotificationDTO model)
        {
            try
            {
                var response = await _notificationRepository.OrderPlacedNotify(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

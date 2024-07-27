using Application.DTOs.Request.OrderEntity;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrderRepository _orderRepository) : Controller
    {

        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder(OrderDTO model)
        {
            var response = await _orderRepository.PlaceOrderAsync(model);
            return Ok(response);
        }

        [HttpGet("get-orders/{userId}")]
        public async Task<IActionResult> GetOrders(string userId)
        {
            var data = await _orderRepository.GetUserOrders(userId);
            return Ok(data);
        }

    }
}

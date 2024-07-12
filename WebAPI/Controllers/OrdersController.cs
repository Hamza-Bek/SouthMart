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
            try
            {
                var response = await _orderRepository.PlaceOrderAsync(model);
                return Ok(response);    
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

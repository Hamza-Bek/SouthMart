using Application.DTOs.Request.OrderEntity;
using Application.DTOs.Response;
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
            var data = await _orderRepository.GetUserOrdersAsync(userId);
            return Ok(data);
        }

		[HttpGet("get-order-orderid/{orderId}")]
		public async Task<IActionResult> GetOrderByOrderId(string orderId)
		{
			var data = await _orderRepository.GetUserOrdersByOrderIdAsync(orderId);
			return Ok(data);
		}


        [HttpDelete("clear-cart-total/{userId}")]
        public async Task<ActionResult<OrderResponse>> ClearCartTotal(string userId)
        {
            try
            {
                var response = await _orderRepository.ClearCartTotalAsync(userId);
                if (!response.Flag)
                {
                    return NotFound(response); // Return 404 Not Found if user or cart not found
                }
                return Ok(response); // Return 200 OK with response object if successful
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}"); // Return 500 Internal Server Error on unexpected errors
            }
        }

        [HttpDelete("clear-cart-items/{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> ClearCartItems(string userId)
        {
            var result = await _orderRepository.ClearCartItemsAsync(userId);
            return Ok(result);
        }

    }
}

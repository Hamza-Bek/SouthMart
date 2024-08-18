using Application.DTOs.Request.OrderEntity;
using Application.DTOs.Response;
using Application.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrderRepository _orderRepository) : Controller
    {

        [HttpPost("place-order")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PlaceOrder(OrderDTO model)
        {
            var response = await _orderRepository.PlaceOrderAsync(model);
            return Ok(response);
        }

        [HttpGet("get-orders/{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetOrders(string userId)
        {
            var data = await _orderRepository.GetUserOrdersAsync(userId);
            return Ok(data);
        }

        [HttpGet("get-all-orders")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllOrders()
        {
            var data = await _orderRepository.GetOrdersAsync();
            return Ok(data);
        }

        [HttpGet("get-order-orderid/{orderId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetOrderByOrderId(string orderId)
		{
			var data = await _orderRepository.GetUserOrdersByOrderIdAsync(orderId);
			return Ok(data);
		}


        [HttpDelete("clear-cart-total/{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
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
        public async Task<IActionResult> ClearCartItems(string userId)
        {
            var result = await _orderRepository.ClearCartItemsAsync(userId);
            return Ok(result);
        }

        [HttpGet("get-product-status-dic")]
        public async Task<IActionResult> GetProductStatuses()
        {
            var response = await _orderRepository.GetProductStatusAsync();
            return Ok(response);
        }

        [HttpGet("get-order-status-dic")]
        public async Task<IActionResult> GetOrderStatuses()
        {
            var response = await _orderRepository.GetOrderStatusAsync();
            return Ok(response);
        }

        [HttpPut("change-product-status/{productId}/{newStatusId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ChangeProductStatus(string productId, string newStatusId)
        {

            var data = await _orderRepository.ChangeProductStatusAsync(productId, newStatusId);
            return Ok(data);
        }

        [HttpPut("change-order-status/{orderId}/{newStatusId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ChangeOrderStatus(string orderId, string newStatusId)
        {

            var data = await _orderRepository.ChangeOrderStatusAsync(orderId, newStatusId);
            return Ok(data);
        }
    }
}

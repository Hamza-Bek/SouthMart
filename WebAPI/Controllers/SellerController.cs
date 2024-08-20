using Application.DTOs.Request;
using Application.DTOs.Request.Account;
using Application.DTOs.Request.ProductEntity;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Models.SellerEntity;
using Domain.Models.UserEntity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController(ISellerRepository _sellerRepository) : Controller
    {


        [HttpPost("approve-seller-account")]        
        public async Task<IActionResult> ApproveSellerAccount([FromBody] ApproveSellerAccountDTO model)
        {
            var response = await _sellerRepository.ApproveSellerAccountAsync(model.AccountId, model.IsApproved);
            if (!response.Flag)
                return BadRequest(response.Message);

            return Ok(response.Message);
        }

        [HttpPost("create-seller-status")]
        public async Task<IActionResult> CreateSellerStatus(SellerStatus model)
        {
            var response = await _sellerRepository.AddSellerStatus(model);
            if (!response.Flag)
                return BadRequest(response.Message);

            return Ok(response.Message);
        }

        [HttpPost("create-seller-account")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateSellerAccountAsync(SellerAccountDTO model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Invalid data.");
                }

                var result = await _sellerRepository.CreateSellerAccountAsync(model);
                if (!result.Flag)
                {
                    return BadRequest(result.Message);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("get-seller-account")]        
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SellerAccountDTO>))]
        public async Task<IActionResult> GetSellerAccount(string userId)
        {
            try
            {
                var data = await _sellerRepository.GetSellerAccountAsync(userId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("last-year-sales")]
        public async Task<ActionResult<SalesResponse>> GetLastYearSales([FromQuery] string sellerId)
        {
            if (string.IsNullOrEmpty(sellerId))
            {
                return BadRequest(new SalesResponse(false, "Seller ID is required", new List<OrderDetails>()));
            }

            var response = await _sellerRepository.GetSalesForLastYearAsync(sellerId);

            if (response.Flag)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(500, response);
            }
        }

        [HttpGet("current-year-sales")]
        public async Task<ActionResult<SalesResponse>> GetCurrentYearSales([FromQuery] string sellerId)
        {
            if (string.IsNullOrEmpty(sellerId))
            {
                return BadRequest(new SalesResponse(false, "Seller ID is required", new List<OrderDetails>()));
            }

            var response = await _sellerRepository.GetSalesForCurrentYearAsync(sellerId);

            if (response.Flag)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(500, response);
            }
        }


        [HttpGet("last-month-sales")]
        public async Task<ActionResult<SalesResponse>> GetLastMonthSales([FromQuery] string sellerId)
        {
            if (string.IsNullOrEmpty(sellerId))
            {
                return BadRequest(new SalesResponse(false, "Seller ID is required", new List<OrderDetails>()));
            }

            var response = await _sellerRepository.GetSalesForLastMonthAsync(sellerId);

            if (response.Flag)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(500, response);
            }
        }

        [HttpGet("current-month-sales")]
        public async Task<ActionResult<SalesResponse>> GetCurrentMonthSales([FromQuery] string sellerId)
        {
            if (string.IsNullOrEmpty(sellerId))
            {
                return BadRequest(new SalesResponse(false, "Seller ID is required", new List<OrderDetails>()));
            }

            var response = await _sellerRepository.GetSalesForCurrentMonthAsync(sellerId);

            if (response.Flag)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(500, response);
            }
        }

        [HttpGet("last-24H-sales")]
        public async Task<ActionResult<SalesResponse>> GetSalesForLast24Hours([FromQuery] string sellerId)
        {
            if (string.IsNullOrEmpty(sellerId))
            {
                return BadRequest(new SalesResponse(false, "Seller ID is required", new List<OrderDetails>()));
            }

            var response = await _sellerRepository.GetSalesForLast24HoursAsync(sellerId);

            if (response.Flag)
            {
                return Ok(response);
            }
            else
            {
                return StatusCode(500, response);
            }
        }

        [HttpGet("expired-product/{sellerId}")]
        public async Task<IActionResult> GetExpiredProduct(string sellerId)
        {
            var data = await _sellerRepository.GetExpiredProductAsync(sellerId);
            return Ok(data);
        }

        [HttpGet("recently-added-product/{sellerId}")]
        public async Task<IActionResult> GetRecentlyAddedProduct(string sellerId)
        {
            var data = await _sellerRepository.GetRecentlyAddedProductAsync(sellerId);
            return Ok(data);
        }

        [HttpGet("get-orders/{sellerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OrderDetails>))]
        public async Task<IActionResult> GetSellerOrders(string sellerId)
        {
            try
            {
                var data = await _sellerRepository.GetSellerOrders(sellerId);

                // Check if no data is found for the seller
                if (data == null || !data.Any())
                {
                    return NotFound(new { message = "No orders found for this seller." });
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging (optional)
                // _logger.LogError(ex, "Error while retrieving seller orders");

                return BadRequest(new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }

        [HttpGet("get-order-id/{orderId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OrderDetails>))]
        public async Task<IActionResult> GetOrderById(string orderId)
        {
            try
            {
                var data = await _sellerRepository.GetOrder(orderId);

                // Check if no data is found for the seller
                if (data == null )
                {
                    return NotFound(new { message = "No orders found for this seller." });
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                // Log the exception for debugging (optional)
                // _logger.LogError(ex, "Error while retrieving seller orders");

                return BadRequest(new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }

        [HttpGet("get-sellers")]
        public async Task<IActionResult> GetAllSellers()
        {
            var data = await _sellerRepository.GetAllSellersAsync();
            return Ok(data);
        }
    }
}

﻿using Application.DTOs.Request.ProductEntity;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuyersController(IBuyerRepository _buyerRepository) : Controller
    {

        [HttpPost("add-product-cart")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductDTO>))]
        public async Task<IActionResult> AddProductToCart(string productId, string userId)
        {
            try
            {
                var response = await _buyerRepository.AddProductToCartAsync(productId, userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

      
        [HttpDelete("remove-product-cart")]
        public async Task<IActionResult> RemoveProductFromCart(string productId , string userId)
        {
            try
            {
                var response = await _buyerRepository.RemoveProductFromCartAsync(productId, userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("get-user-cart")]
        public async Task<IActionResult> GetCart(string userId)
        {
            try
            {
                var data = await _buyerRepository.GetCartAsync(userId);
                return Ok(data);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
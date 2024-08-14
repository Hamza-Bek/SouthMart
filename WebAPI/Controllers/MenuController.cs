
using Application.DTOs.Request.ProductEntity;
using Application.Extensions;
using Application.Interfaces;
using AutoMapper;
using Domain.Models.ProductEntity;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController(AppDbContext _context , IMenuRepository _menuRepository , IMapper _mapper) : Controller
    {
        [HttpGet("get-categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = _context.Categories.ToList();
            // var categoriesDic = categories.ToDictionary(e => e.CategoryId, e => e.CategoryTag);
            return Ok(categories);
        }

        [HttpGet("get-products")]
        public async Task<IActionResult> GetProducts()
        {
            var products = _context.Products.ToList();
            // var categoriesDic = categories.ToDictionary(e => e.CategoryId, e => e.CategoryTag);
            return Ok(products);
        }
     
        [HttpGet("get-category-products/{categoryTag}")]
        public async Task<IActionResult> GetProductsByCategory(string categoryTag)
        {
            try
            {
                var data = await _menuRepository.GetProductsByCategoryAsync(categoryTag);
                var mappedPlates = _mapper.Map<IEnumerable<ProductDTO>>(data);
                return Ok(mappedPlates);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-product-by-Id/{product}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        public async Task<IActionResult> GetProductById(string product)
        {
            try
            {
                var data = await _menuRepository.GetProductByIdAsync(product);
                var mappedPlates = _mapper.Map<IEnumerable<ProductDTO>>(data);
                return Ok(mappedPlates);                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-product-by-seller/{userId}")]
        public async Task<IActionResult> GetProductBySeller(string userId)
        {
            try
            {
                var response = await _menuRepository.GetProductBySeller(userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-top-selling-products")]
        public async Task<IActionResult> GetTopSellingProducts()
        {
            try
            {
                var data = await _menuRepository.GetTopSellingProductsAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("get-new-products")]
        public async Task<IActionResult> GetNewestProducts()
        {
            try
            {
                var data = await _menuRepository.GetNewestProductsAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("get-random-products")]
        public async Task<IActionResult> GetRandomProducts()
        {
            try
            {
                var data = await _menuRepository.GetRandomProductsAsync();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}

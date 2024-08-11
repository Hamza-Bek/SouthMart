using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController(AppDbContext _context , IMenuRepository _menuRepository) : Controller
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
                var response = await _menuRepository.GetProductsByCategoryAsync(categoryTag);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-product-by-Id/{product}")]
        public async Task<IActionResult> GetProductById(string product)
        {
            try
            {
                var response = await _menuRepository.GetProductByIdAsync(product);
                return Ok(response);
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

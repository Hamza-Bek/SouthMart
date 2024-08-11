using Application.DTOs.Request.ProductEntity;
using Application.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController (IProductRepository _productRepository , AppDbContext _context) : Controller
    {
        [HttpPost("add-product")]        
        public async Task<IActionResult> AddProduct(ProductDTO model)
        {
            try
            {
                var response = await _productRepository.AddProductAsync(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update-product")]        
        public async Task<IActionResult> UpdateProductAsync(ProductDTO model)
        {
            try
            {
                var response = await _productRepository.UpdateProductAsync(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("remove-product/{productId}")]        
        public async Task<IActionResult> RemoveProductAsync(string productId)
        {
            try
            {
                var response = await _productRepository.RemoveProductAsync(productId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }       

        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategory(CategoryDTO model)
        {
            try
            {
                var response = await _productRepository.AddCategoryAsync(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-category-dic")]
        public async Task<IActionResult> GetCategories()
        {
            var response = await _productRepository.GetCategoriesDicAsync();
            return Ok(response);
        }
    }
}

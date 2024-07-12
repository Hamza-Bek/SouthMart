using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Models.SellerEntity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController(ISellerRepository _sellerRepository) : Controller
    {


        [HttpPost("create-seller-account")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateSellerAccountAsync(SellerAccountDTO model)
        {
            try
            {
                var result = await _sellerRepository.CreateSellerAccountAsync(model);
                return Ok(result);
            }
            catch(Exception ex)
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
    }
}

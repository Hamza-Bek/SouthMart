using Application.DTOs.Request.OrderEntity;
using Application.Interfaces;
using Domain.Models.UserEntity;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController(ILocationRepository _locationRepository) : Controller
    {
        [HttpPost("add-location")]
        public async Task<IActionResult> AddLocation(LocationDTO model)
        {
            try
            {
                var response = await _locationRepository.AddLocationAsync(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("edit-location")]
        public async Task<IActionResult> EditLocation(LocationDTO model)
        {
            try
            {
                var response = await _locationRepository.UpdateLocationAsync(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-location/{userId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Location>))]
        public async Task<IActionResult> GetOrder(string userId)
        {
            var data = await _locationRepository.GetLocation(userId);
            return Ok(data);
        }
    }
}

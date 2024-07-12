using Application.DTOs.Request.OrderEntity;
using Application.Interfaces;
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
    }
}

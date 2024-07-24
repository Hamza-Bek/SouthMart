using Application.DTOs.Request.OrderEntity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController
    {

        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder(OrderDTO model)
        {
            throw new NotImplementedException();
        }

    }
}

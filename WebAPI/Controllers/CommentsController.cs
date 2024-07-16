using Application.DTOs.Request.ProductEntity;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController(ICommentRepository _commentRepository) : Controller
    {
        [HttpPost]
        public async Task<IActionResult> AddComment(CommentDTO model)
        {
            try
            {
                var response = await _commentRepository.AddCommentAsync(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

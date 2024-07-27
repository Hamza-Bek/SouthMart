using Application.DTOs.Request.ProductEntity;
using Application.Interfaces;
using Application.Services;
using Domain.Models.ProductEntity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController(ICommentRepository _commentRepository) : Controller
    {
        
        [HttpPost("add-comment")]
        public async Task<IActionResult> AddComment(CommentDTO comment)
        {
            if (comment == null)
            {
                return BadRequest("Invalid comment data.");
            }

            // Validate the comment model
            if (string.IsNullOrEmpty(comment.content) || string.IsNullOrEmpty(comment.ProductId) || string.IsNullOrEmpty(comment.UserId))
            {
                return BadRequest("Comment content, ProductId, or UserId cannot be empty.");
            }

            // Add comment logic here
            var result = await _commentRepository.AddCommentAsync(comment);

            if (result.Flag)
            {
                return Ok(new { Flag = true, Message = "Comment added successfully." });
            }
            else
            {
                return StatusCode(500, "An error occurred while adding the comment.");
            }
        }

        [HttpPut("edit-comment")]
        public async Task<IActionResult> EditComment(CommentDTO model)
        {
            try
            {
                var response = await _commentRepository.EditCommentAsync(model);
                return Ok(response);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-comments/{productId}")]
        public async Task<IActionResult> GetComments(string productId)
        {
            try
            {
                var data = await _commentRepository.GetProductCommentsAsync(productId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

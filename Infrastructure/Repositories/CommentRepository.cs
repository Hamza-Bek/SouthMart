using Application.DTOs.Request.ProductEntity;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Models.ProductEntity;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class CommentRepository (AppDbContext _context , IMapper _mapper) : ICommentRepository
    {
      
        public async Task<NotificationResponse> AddCommentAsync(CommentDTO model)
        {
            _mapper.Map<CommentDTO>(model);

            var user = await _context.Users.FindAsync(model.UserId);
            if (user == null)
                return new NotificationResponse(false, "User not found, Can't add comment");

            if (model == null)
                return new NotificationResponse(false, "Can't send null comment");

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == model.ProductId);
            if (product == null)
                return new NotificationResponse(false, "Product not selected , or not found");

            var comment = new Comment()
            {
                Id = Guid.NewGuid().ToString(),
                content = model.content,
                DateCreated = DateTime.Now,
                UserId = model.UserId,
                User = user,
                ProductId = model.ProductId,
                Product = product,
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return new NotificationResponse(true, "Comment posted successfully"); 
        }

        public async Task<NotificationResponse> EditCommentAsync(CommentDTO model)
        {
            if (model == null)
                return new NotificationResponse(false, "Can't update null comment");

            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == model.Id);
            if (comment == null)
                return new NotificationResponse(false, "Comment not found");

            if (comment.UserId != model.UserId)
                return new NotificationResponse(false, "User not authorized to edit this comment");

            comment.content = model.content;
            comment.DateCreated = DateTime.Now; // Assuming you have a DateModified field for tracking edits

            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();

            return new NotificationResponse(true, "Comment updated successfully");
        }

        public async Task<List<Comment>> GetProductCommentsAsync(string productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(ui => ui.Id == productId);
            if (product == null)
                return new List<Comment>();

            var comments = await _context.Comments
                .Include(u => u.User)
                .Where(ui => ui.ProductId == productId)
                .ToListAsync();

            return comments;
        }
    }
}


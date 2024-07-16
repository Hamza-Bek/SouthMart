using Application.DTOs.Request.ProductEntity;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Models.ProductEntity;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CommentRepository(AppDbContext _context , IMapper _mapper) : ICommentRepository
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
                ProductId = model.ProductId,
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return new NotificationResponse(true, "Comment posted successfully"); 
        }
    }
}

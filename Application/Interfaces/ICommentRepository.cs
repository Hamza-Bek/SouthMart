using Application.DTOs.Request.ProductEntity;
using Application.DTOs.Response;
using Domain.Models.ProductEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICommentRepository
    {
        //POST's METHODS
        Task<NotificationResponse> AddCommentAsync(CommentDTO model);
        Task<NotificationResponse> EditCommentAsync(CommentDTO model);
        //Task<NotificationResponse> DeleteCommentAsync(CommentDTO model);

        //GET's METHODS
        Task<List<Comment>> GetProductCommentsAsync(string productId);
    }
}

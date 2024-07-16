using Application.DTOs.Request.ProductEntity;
using Application.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICommentRepository
    {
        Task<NotificationResponse> AddCommentAsync(CommentDTO model);
        //Task<NotificationResponse> EditCommentAsync(CommentDTO model);
        //Task<NotificationResponse> DeleteCommentAsync(CommentDTO model);

        //Task<NotificationResponse> GetProductCommentsAsync(string productId);
    }
}

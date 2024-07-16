using Domain.Models.Authentication;
using Domain.Models.ProductEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.ProductEntity
{
    public class CommentDTO
    {
        public string Id { get; set; }
        public string content { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserId { get; set; }        
        public string ProductId { get; set; }        
    }
}

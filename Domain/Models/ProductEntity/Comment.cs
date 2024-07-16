using Domain.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ProductEntity
{
    public class Comment
    {
        public string Id { get; set; }
        public string content { get; set; }
        public DateTime DateCreated { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }  
        public string ProductId { get; set; }
        public Product Product { get; set; }
    }
}

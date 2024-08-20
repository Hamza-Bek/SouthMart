using Domain.Models.ProductEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Authentication
{
    public class Like
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
        public Product Product { get; set; }
        public Image Image { get; set; }
    }
}

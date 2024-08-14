using Domain.Models.ProductEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.UserEntity
{
    public class CartItem
    {
        public string Id { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal Total { get; set; }
        public int Quantity { get; set; }
        public string? CoverImageUrl { get; set; }

        public string CartId { get; set; }
        public Cart? Cart { get; set; }

        public string ProductId { get; set; }
        public Product Product { get; set; }

    }
}

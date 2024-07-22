using Domain.Models.Authentication;
using Domain.Models.UserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.ProductEntity
{
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Quantity { get; set; }
        public decimal? Cost { get; set; }
        public decimal? SellingPrice { get; set; }
        public string? Category { get; set; }
        public string SellerId { get; set; }
        public ApplicationUser Seller { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();  // Navigation property to Comments
    }
}

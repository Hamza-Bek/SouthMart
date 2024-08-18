using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.UserEntity
{
    public class OrderDetails
    {
        [Key]
        public string OrderDetailId { get; set; }
        public string OrderId { get; set; }
        public Order Order { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string SellerId { get; set; }
        public string ProductStatus { get; set; }
        public string? CoverImageUrl { get; set; }
        public int? Quantity { get; set; }
    }
}

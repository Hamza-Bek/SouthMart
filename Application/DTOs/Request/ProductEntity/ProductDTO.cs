using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.ProductEntity
{
    public class ProductDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Quantity { get; set; }
        public string? Category { get; set; }
        public decimal? Cost { get; set; }
        public decimal? SellingPrice { get; set; }
        public string SellerId { get; set; }
        public int TotalSold { get; set; }
        public DateTime AddedDate { get; set; }
    }
}

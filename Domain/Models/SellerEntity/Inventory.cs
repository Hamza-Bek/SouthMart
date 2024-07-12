using Domain.Models.ProductEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.SellerEntity
{
    public class Inventory
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public int? Quantity { get; set; }
        public string? Description { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? Cost { get; set; }
      
    }
}

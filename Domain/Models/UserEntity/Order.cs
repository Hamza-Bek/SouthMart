using Domain.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.UserEntity
{
    public class Order
    {
        public string OrderId { get; set; }
        public string? OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderTotal { get; set; }
        public string? OrderStatus { get; set; }
        public string SellerId { get; set; }

        // RELATIONSHIPS                
        public string? LocationId { get; set; }
        public Location? Location { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser OrderMaker { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();

        
    }
}

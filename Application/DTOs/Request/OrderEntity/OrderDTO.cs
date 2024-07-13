using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.OrderEntity
{
    public class OrderDTO
    {

         public string OrderId { get; set; }
        public string? OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderTotal { get; set; }
        public string? OrderStatus { get; set; }
        public string SellerId { get; set; }

        // RELATIONSHIPS                
        public string? LocationId { get; set; }        
        public string? UserId { get; set; }

    }
}

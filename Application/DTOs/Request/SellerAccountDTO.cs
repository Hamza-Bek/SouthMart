using Domain.Models.SellerEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class SellerAccountDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal GrossSales { get; set; }
        public decimal Revenue { get; set; }
        public string SellerId { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; }
    }
}

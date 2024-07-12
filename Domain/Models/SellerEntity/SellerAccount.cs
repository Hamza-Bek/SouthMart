using Domain.Models.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.SellerEntity
{
    public class SellerAccount
    {

        public required string Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; }
        public decimal GrossSales { get; set; }
        public decimal Revenue { get; set; }


        // Foreign key for ApplicationUser
        public string? SellerId { get; set; }
        public ApplicationUser Seller { get; set; }

    
    }
}

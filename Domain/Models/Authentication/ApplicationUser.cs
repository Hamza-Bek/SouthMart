using Domain.Models.ProductEntity;
using Domain.Models.SellerEntity;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }

        public SellerAccount SellerAccount { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}

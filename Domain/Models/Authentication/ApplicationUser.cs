using Domain.Models.ProductEntity;
using Domain.Models.SellerEntity;
using Domain.Models.UserEntity;
using Microsoft.AspNetCore.Identity;

namespace Domain.Models.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }

        public SellerAccount SellerAccount { get; set; }

        public string CartId { get; set; }
        public Cart Cart { get; set; }


        public Location Location { get; set; }
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}

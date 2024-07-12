using Domain.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.UserEntity
{
    public class Cart
    {
        public string Id { get; set; }
        public decimal? CartTotal { get; set; }


        public string? UserId { get; set; }
        public ApplicationUser CartOwner { get; set; }

        public ICollection<CartItem>? CartItems { get; set; } = new List<CartItem>();


    }
}

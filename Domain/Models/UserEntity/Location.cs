using Domain.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.UserEntity
{
    public class Location
    {
        public string LocationId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Country { get; set; }
        public string? Street { get; set; }
        public string? Building { get; set; }
        public string? Floor { get; set; }

        public string ApplicationUserId { get; set; } // Foreign key property
        public ApplicationUser LocationUser { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}

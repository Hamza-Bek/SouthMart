using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Authentication
{
    public class Notification
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        // Seller's proprites
        public string? ReferenceId { get; set; }
        public int? ThresholdQuantity { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Account
{
    public class NotificationDTO
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }

        // Seller's proprties
        public string? ReferenceId { get; set; }
        public int? ThresholdQuantity { get; set; }
    }
}

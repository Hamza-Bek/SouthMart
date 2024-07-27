using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.SellerEntity
{
    public class SellerStatus
    {
        [Key]
        public string Approved { get; set; }
        public string Rejected { get; set; }
    }
}

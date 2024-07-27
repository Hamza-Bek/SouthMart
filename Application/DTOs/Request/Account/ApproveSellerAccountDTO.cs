using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request.Account
{
    public class ApproveSellerAccountDTO
    {  
        public string AccountId { get; set; }
        public bool IsApproved { get; set; }
    }
}

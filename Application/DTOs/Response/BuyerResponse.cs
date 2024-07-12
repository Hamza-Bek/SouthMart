using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public record BuyerResponse(bool Flag = false, string Message = null!);
}

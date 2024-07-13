using Domain.Models.SellerEntity;
using Domain.Models.UserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public record SalesResponse(bool Flag = false, string Message = null!, List<OrderDetails> SalesData = null!);
}

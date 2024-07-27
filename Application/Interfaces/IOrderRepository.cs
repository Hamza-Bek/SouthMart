using Application.DTOs.Request.OrderEntity;
using Application.DTOs.Response;
using Domain.Models.UserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<OrderResponse> PlaceOrderAsync(OrderDTO model);

        // GET's METHODS
        Task<IEnumerable<OrderDTO>> GetUserOrders(string userId);
    }
}

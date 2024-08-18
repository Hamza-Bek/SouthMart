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
      
        Task<OrderResponse> ClearCartTotalAsync(string userId);
      
        Task<OrderResponse> ClearCartItemsAsync(string userId);
       
        Task<OrderResponse> ChangeProductStatusAsync(string productId, string newStatusId);
        Task<OrderResponse> ChangeOrderStatusAsync(string orderId, string newStatusId);

        // GET's METHODS
        Task<Dictionary<string, string>> GetProductStatusAsync();
        Task<Dictionary<string, string>> GetOrderStatusAsync();

        Task<IEnumerable<OrderDTO>> GetUserOrdersAsync(string userId);

        Task<IEnumerable<Order>> GetOrdersAsync();

        Task<IEnumerable<Order>> GetUserOrdersByOrderIdAsync(string orderId);
	}
}

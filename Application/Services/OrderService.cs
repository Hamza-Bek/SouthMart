using Application.DTOs.Request.OrderEntity;
using Application.DTOs.Response;
using Application.Extensions;
using Application.Interfaces;
using Domain.Models.ProductEntity;
using Domain.Models.UserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class OrderService : IOrderRepository
    {

        private readonly HttpClient _httpClient;
        private readonly HttpClientService httpClientService;
        public OrderService(HttpClient httpClient, HttpClientService httpClientService)
        {
            _httpClient = httpClient;
            this.httpClientService = httpClientService;
        }

        public async Task<OrderResponse> ChangeOrderStatusAsync(string orderId, string newStatusId)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { newStatusId }), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/orders/change-order-status/{orderId}/{newStatusId}", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new OrderResponse(false, $"Error changing order status: {errorContent}");
            }

            var result = await response.Content.ReadFromJsonAsync<OrderResponse>();
            return result!;
        }

        public async Task<OrderResponse> ChangeProductStatusAsync(string productId, string newStatusId)
        {
            var content = new StringContent(JsonSerializer.Serialize(new { newStatusId }), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/orders/change-product-status/{productId}/{newStatusId}", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new OrderResponse(false, $"Error changing order status: {errorContent}");
            }

            var result = await response.Content.ReadFromJsonAsync<OrderResponse>();
            return result!;
        }

        public async Task<OrderResponse> ClearCartItemsAsync(string userId)
        {
            var response = await _httpClient.DeleteAsync($"api/orders/clear-cart-items/{userId}");
            if (response.IsSuccessStatusCode)
            {
                return new OrderResponse { Flag = true, Message = "Item deleted successfully." };
            }
            else
            {
                return new OrderResponse { Flag = false, Message = "Failed to remove the plate!" };
            }
        }

        public async Task<OrderResponse> ClearCartTotalAsync(string userId)
        {
            var response = await _httpClient.DeleteAsync($"api/orders/clear-cart-total/{userId}");
            if (response.IsSuccessStatusCode)
            {
                return new OrderResponse { Flag = true, Message = "Item deleted successfully." };
            }
            else
            {
                return new OrderResponse { Flag = false, Message = "Failed to remove the plate!" };
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/orders/get-all-orders");
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadFromJsonAsync<IEnumerable<Order>>();
                return data ?? Enumerable.Empty<Order>();
            }
            catch
            {
                return Enumerable.Empty<Order>();
            }
        }

        public async Task<Dictionary<string, string>> GetOrderStatusAsync()
        {
            var response = await _httpClient.GetAsync("api/orders/get-order-status-dic");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                return data!;
            }
            throw new Exception();
        }

        public async Task<Dictionary<string, string>> GetProductStatusAsync()
        {
            var response = await _httpClient.GetAsync("api/orders/get-product-status-dic");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                return data!;
            }
            throw new Exception();
        }

        public async Task<IEnumerable<OrderDTO>> GetUserOrdersAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/orders/get-orders/{userId}");
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadFromJsonAsync<IEnumerable<OrderDTO>>();
                return data ?? Enumerable.Empty<OrderDTO>();
            }
            catch
            {
                return Enumerable.Empty<OrderDTO>();
            }
        }

		public async Task<IEnumerable<Order>> GetUserOrdersByOrderIdAsync(string orderId)
		{
            try
            {
                var response = await _httpClient.GetAsync($"api/orders/get-order-orderid/{orderId}");
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadFromJsonAsync<IEnumerable<Order>>();
                return data ?? Enumerable.Empty<Order>();
            }
            catch
            {
                return Enumerable.Empty<Order>();
            }
		}

		public async Task<OrderResponse> PlaceOrderAsync(OrderDTO model)
        {
            try
            {
                 model = new OrderDTO()
                {
                    OrderId = Guid.NewGuid().ToString(),
                    OrderDate = DateTime.Now,
                    OrderStatus = "Pending",
                    OrderTotal = 0,
                    UserId = model.UserId,
                    OrderNumber = "0",
                    LocationId = "0"
                };

                var data = await _httpClient.PostAsJsonAsync("api/orders/place-order", model);
                var response = await data.Content.ReadFromJsonAsync<OrderResponse>();
                
                if (response.Flag)
                    return new OrderResponse(true, "Order Placed Successfully");
                else
                    return new OrderResponse(false, "Something went wrong while placing the order");
            }
            catch (Exception ex)
            {
                return new OrderResponse (false , ex.Message );
            }
        }
    }
}

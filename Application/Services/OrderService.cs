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

        public async Task<IEnumerable<OrderDTO>> GetUserOrders(string userId)
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

        public async Task<OrderResponse> PlaceOrderAsync(OrderDTO model)
        {
            try
            {
                var data = await _httpClient.PostAsJsonAsync("", model);
                var responseContent = await data.Content.ReadAsStringAsync();

                var response = JsonSerializer.Deserialize<OrderResponse>(responseContent);
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

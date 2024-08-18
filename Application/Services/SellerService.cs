using Application.DTOs.Request;
using Application.DTOs.Request.Account;
using Application.DTOs.Request.ProductEntity;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Models.Authentication;
using Domain.Models.ProductEntity;
using Domain.Models.SellerEntity;
using Domain.Models.UserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class SellerService (HttpClient _httpClient) : ISellerRepository
    {
        public async Task<SellerResponse> AddSellerStatus(SellerStatus model)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/seller/create-seller-status", model);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<SellerResponse>();
            }
            catch (Exception ex)
            {
                return new SellerResponse(false, ex.Message);
            }
        }

        public async Task<SellerResponse> ApproveSellerAccountAsync(string userId, bool isApproved)
        {
            try
            {
                var model = new ApproveSellerAccountDTO { AccountId = userId, IsApproved = isApproved };
                var response = await _httpClient.PostAsJsonAsync("api/seller/approve-seller-account", model);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<SellerResponse>();
            }
            catch (Exception ex)
            {
                return new SellerResponse(false, ex.Message);
            }
        }

        public Task<SellerResponse> ChangeUserRoleToSeller(ApplicationUser user)
        {
            // Implement this method as needed
            throw new NotImplementedException();
        }

        public async Task<SellerResponse> CreateSellerAccountAsync(SellerAccountDTO model)
        {
            try
            {
                if (model == null)
                {
                    return new SellerResponse(false, "Invalid data.");
                }

                model.Id = Guid.NewGuid().ToString();
                model.DateCreated = DateTime.Now;
                model.GrossSales = 0;
                model.Revenue = 0;
                model.Status = "Pending";

                var response = await _httpClient.PostAsJsonAsync("api/seller/create-seller-account", model);

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<SellerResponse>();
            }
            catch (HttpRequestException httpEx)
            {
                return new SellerResponse(false, $"Request error: {httpEx.Message}");
            }
            catch (JsonException jsonEx)
            {
                return new SellerResponse(false, $"JSON error: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                return new SellerResponse(false, $"General error: {ex.Message}");
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetExpiredProductAsync(string sellerId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/seller/expired-product/{sellerId}");
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadFromJsonAsync<List<ProductDTO>>();
                return data ?? new List<ProductDTO>();
            }
            catch
            {
                // Log the exception (ex)
                return new List<ProductDTO>();
            }
        }

        public async Task<IEnumerable<OrderDetails>> GetOrder(string orderId)
        {
            try
            {
                // Fetch the data from the API
                var response = await _httpClient.GetFromJsonAsync<IEnumerable<OrderDetails>>($"api/seller/get-order-id/{orderId}");

                // Return the response wrapped in a collection
                return (IEnumerable<OrderDetails>)response;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error fetching order details: {ex.Message}");
                return null; // Return an empty collection if there's an error
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetRecentlyAddedProductAsync(string sellerId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<IEnumerable<ProductDTO>>($"api/seller/recently-added-product/{sellerId}");
                if (response == null)
                {
                    return Enumerable.Empty<ProductDTO>();
                }

                return response;
            }
            catch (Exception ex)
            {
                return Enumerable.Empty<ProductDTO>();
            }
        }

        public async Task<SalesResponse> GetSalesForCurrentMonthAsync(string sellerId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<SalesResponse>($"api/seller/current-month-sales?sellerId={sellerId}");
                return response;
            }
            catch (Exception ex)
            {
                return new SalesResponse(false, ex.Message, new List<OrderDetails>());
            }
        }

        public async Task<SalesResponse> GetSalesForCurrentYearAsync(string sellerId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<SalesResponse>($"api/seller/current-year-sales?sellerId={sellerId}");
                return response;
            }
            catch (Exception ex)
            {
                return new SalesResponse(false, ex.Message, new List<OrderDetails>());
            }
        }

        public async Task<SalesResponse> GetSalesForLast24HoursAsync(string sellerId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<SalesResponse>($"api/seller/last-24H-sales?sellerId={sellerId}");
                return response;
            }
            catch (Exception ex)
            {
                return new SalesResponse(false, ex.Message, new List<OrderDetails>());
            }
        }

        public async Task<SalesResponse> GetSalesForLastMonthAsync(string sellerId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<SalesResponse>($"api/seller/last-month-sales?sellerId={sellerId}");
                return response;
            }
            catch (Exception ex)
            {
                return new SalesResponse(false, ex.Message, new List<OrderDetails>());
            }
        }

        public async Task<SalesResponse> GetSalesForLastYearAsync(string sellerId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<SalesResponse>($"api/seller/last-year-sales?sellerId={sellerId}");
                return response;
            }
            catch (Exception ex)
            {
                return new SalesResponse(false, ex.Message, new List<OrderDetails>());
            }
        }

        public async Task<SellerAccountDTO> GetSellerAccountAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<SellerAccountDTO>($"api/seller/get-seller-account?userId={userId}");
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<OrderDetails>> GetSellerOrders(string sellerId)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<IEnumerable<OrderDetails>>($"api/seller/get-orders/{sellerId}");
                return response ?? Enumerable.Empty<OrderDetails>(); // Return an empty collection if the response is null
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("Error fetching seller orders: " + ex.Message);
            }
        }
    }
}

using Application.DTOs.Request.ProductEntity;
using Application.DTOs.Response;
using Application.Extensions;
using Application.Interfaces;
using Domain.Models.Authentication;
using Domain.Models.ProductEntity;
using Domain.Models.UserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.Services
{
    public class BuyerService : IBuyerRepository
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClientService httpClientService;
        public event Action OnChange;

        public BuyerService(HttpClient httpClient, HttpClientService httpClientService)
        {
            _httpClient = httpClient;
            this.httpClientService = httpClientService;
        }

        public async Task<BuyerResponse> AddProductToCartAsync(string productId, string userId)
        {
            try
            {
               
                var data = await _httpClient.PostAsJsonAsync($"api/buyers/add-product-cart/{productId}/{userId}", new { });
                var response = await data.Content.ReadFromJsonAsync<BuyerResponse>();
                if (response.Flag)
                {
                    NotifyStateChanged();
                    return new BuyerResponse { Flag = true, Message = "Product added successfully" };

                }
                else
                {

                    return new BuyerResponse { Flag = false, Message = "Failed to add product" };

                }


            }
            catch (Exception ex) { return new BuyerResponse(Flag: false, Message: ex.Message); }
        }

        public async Task<Cart> GetCartAsync(string userId)
        {
            try
            {

                var response = await _httpClient.GetAsync($"api/buyers/get-user-cart/{userId}");
                string error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    throw new Exception(error);

                var result = await response.Content.ReadFromJsonAsync<Cart>();
                return result!;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BuyerResponse> RemoveProductFromCartAsync(string productId, string userId)
        {
            try
            {

                var response = await _httpClient.DeleteAsync($"api/buyers/remove-product/{productId}/{userId}");
                
                if (response.IsSuccessStatusCode)
                {

                    return new BuyerResponse { Flag = true, Message = "Item deleted successfully." };
                }
                else
                {
                    return new BuyerResponse { Flag = false, Message = "Failed to remove the plate!" };
                }
            }
            catch (Exception ex)
            {
                return new BuyerResponse { Flag = false, Message = ex.Message };
            }
        }

        private static string CheckResponseStatus(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return $"sorry unkown error occured.{Environment.NewLine} Error Description : {Environment.NewLine} Status Code : {response.StatusCode}{Environment.NewLine} Reason Phrase : {response.ReasonPhrase}";
            else
                return null!;
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId)
        {
            try
            {

                var response = await _httpClient.GetAsync($"api/buyers/get-cart-items/{userId}");
                string error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    throw new Exception(error);

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<CartItem>>();
                return result!;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BuyerResponse> LikeProductAsync(string productId, string userId)
        {
            try
            {
                var data = await _httpClient.PostAsJsonAsync($"api/buyers/like-product/{productId}/{userId}", new { });
                var response = await data.Content.ReadFromJsonAsync<BuyerResponse>();
                
                if (response.Flag)
                {
                    return new BuyerResponse { Flag = true, Message = "Product liked successfully" };

                }
                else
                {

                    return new BuyerResponse { Flag = false, Message = "Failed to like product" };

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetLikedProductsAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/buyers/get-likes/{userId}");
                string error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    throw new Exception(error);

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDTO>>();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async void NotifyStateChanged()
        {
            Console.WriteLine("OnChange event triggered"); // Debug output
            OnChange?.Invoke();
        }
    }
}

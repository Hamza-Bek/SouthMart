using Application.DTOs.Response;
using Application.Extensions;
using Application.Interfaces;
using Domain.Models.UserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BuyerService : IBuyerRepository
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClientService httpClientService;


        public BuyerService(HttpClient httpClient, HttpClientService httpClientService)
        {
            _httpClient = httpClient;
            this.httpClientService = httpClientService;
        }
        public Task<BuyerResponse> AddProductToCartAsync(string productId, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Cart> GetCartAsync(string userId)
        {
            try
            {

                var response = await _httpClient.GetAsync($"get-user-cart/{userId}");
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

        public Task<BuyerResponse> RemoveProductFromCartAsync(string productId, string userId)
        {
            throw new NotImplementedException();
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
    }
}

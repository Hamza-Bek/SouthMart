using Application.DTOs.Request.ProductEntity;
using Application.DTOs.Response;
using Application.Extensions;
using Application.Interfaces;
using Domain.Models.ProductEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductRepository
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClientService httpClientService;


        public ProductService(HttpClient httpClient, HttpClientService httpClientService)
        {
            _httpClient = httpClient;
            this.httpClientService = httpClientService;
        }

        public Task<ProductResponse> AddCategoryAsync(CategoryDTO model)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductResponse> AddProductAsync(ProductDTO model)
        {
            try
            {
                var data = await _httpClient.PostAsJsonAsync("api/products/add-product", model);
                var response = await data.Content.ReadFromJsonAsync<ProductResponse>();
                
                if (response.Flag)
                {
                    return new ProductResponse { Flag = true, Message = "Plate added successfully" };
                }
                else
                {
                    return new ProductResponse { Flag = false, Message = "Failed to add plate" };
                }

            }

            catch (Exception ex) 
            { 
                return new ProductResponse(Flag: false, Message: ex.Message);             
            }
        }

        public async Task<ProductResponse> RemoveProductAsync(string productId)
        {
            var response = await _httpClient.DeleteAsync($"api/products/remove-product/{productId}");
            if (response.IsSuccessStatusCode)
            {
                return new ProductResponse { Flag = true, Message = "Item deleted successfully." };
            }
            else
            {
                return new ProductResponse { Flag = false, Message = "Failed to remove the plate!" };
            }
        }

        public async Task<ProductResponse> UpdateProductAsync(ProductDTO model)
        {
            try
            {
                var data = await _httpClient.PutAsJsonAsync("api/products/update-product", model);
                var response = await data.Content.ReadFromJsonAsync<ProductResponse>();

                if (response.Flag)
                {
                    return new ProductResponse { Flag = true, Message = "Product updated successfully" };
                }
                else
                {
                    return new ProductResponse { Flag = false, Message = "Failed to edit Product" };
                }

            }

            catch (Exception ex)
            {
                return new ProductResponse(Flag: false, Message: ex.Message);
            }
        }

        public async Task<Dictionary<string, string>> GetCategoriesDicAsync()
        {
            var response = await _httpClient.GetAsync("api/products/get-category-dic");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                return data!;
            }
            throw new Exception();
        }
    }
}

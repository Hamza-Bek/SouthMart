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

        public Task<ProductResponse> AddProductAsync(ProductDTO model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/products/get-categories");
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadFromJsonAsync<IEnumerable<CategoryDTO>>();
                return data ?? Enumerable.Empty<CategoryDTO>();
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return Enumerable.Empty<CategoryDTO>();
            }
        }

        public Task<List<ProductDTO>> GetProductBySeller(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/products/get-products");
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDTO>>();
                return data ?? Enumerable.Empty<ProductDTO>();
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return Enumerable.Empty<ProductDTO>();
            }
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryTag)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/products/get-category-products/{categoryTag}");
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
                return data ?? Enumerable.Empty<Product>();
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return Enumerable.Empty<Product>();
            }
        }

        public Task<ProductResponse> RemoveProductAsync(string productId)
        {
            throw new NotImplementedException();
        }

        public Task<ProductResponse> UpdateProductAsync(ProductDTO model)
        {
            throw new NotImplementedException();
        }
    }
}

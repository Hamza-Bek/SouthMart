using Application.DTOs.Request.ProductEntity;
using Application.Interfaces;
using Domain.Models.ProductEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class MenuService(HttpClient _httpClient) : IMenuRepository
    {

        public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/menu/get-categories");
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

        public async Task<IEnumerable<ProductDTO>> GetProductByIdAsync(string product)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/menu/get-product-by-Id/{product}");
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDTO>>();
                return data ?? Enumerable.Empty<ProductDTO>();
            }
            catch
            {
                return Enumerable.Empty<ProductDTO>();
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetProductBySeller(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/menu/get-product-by-seller/{userId}");
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadFromJsonAsync<IEnumerable<ProductDTO>>();
                return data ?? Enumerable.Empty<ProductDTO>();
            }
            catch
            {
                return Enumerable.Empty<ProductDTO>();
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/menu/get-products");
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

        public async Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(string categoryTag)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/menu/get-category-products/{categoryTag}");
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

        public async Task<IEnumerable<ProductDTO>> GetTopSellingProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/menu/get-top-selling-products");
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

        public async Task<IEnumerable<ProductDTO>> GetNewestProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/menu/get-new-products");
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

        public async Task<IEnumerable<ProductDTO>> GetRandomProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/menu/get-random-products");
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
    }
}

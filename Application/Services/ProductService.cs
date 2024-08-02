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

using Application.DTOs.Request.ProductEntity;
using Domain.Models.ProductEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IMenuRepository
    {
        Task<List<ProductDTO>> GetProductBySeller(string userId);
        Task<IEnumerable<CategoryDTO>> GetAllCategories();
        Task<IEnumerable<ProductDTO>> GetProducts();
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryTag);
        Task<IEnumerable<Product>> GetProductByIdAsync(string product);

        // algorithms
        Task<IEnumerable<ProductDTO>> GetRandomProductsAsync();
        Task<IEnumerable<ProductDTO>> GetTopSellingProductsAsync();
        Task<IEnumerable<ProductDTO>> GetNewestProductsAsync();
    }
}

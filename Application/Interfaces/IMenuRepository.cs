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
        Task<IEnumerable<ProductDTO>> GetProductBySeller(string userId);
        Task<IEnumerable<CategoryDTO>> GetAllCategories();
        Task<IEnumerable<ProductDTO>> GetProducts();
        Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(string categoryTag);
        Task<IEnumerable<ProductDTO>> GetProductByIdAsync(string product);

        // algorithms
        Task<IEnumerable<ProductDTO>> GetRandomProductsAsync();
        Task<IEnumerable<ProductDTO>> GetTopSellingProductsAsync();
        Task<IEnumerable<ProductDTO>> GetNewestProductsAsync();
    }
}

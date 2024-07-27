using Application.DTOs.Request.ProductEntity;
using Application.DTOs.Response;
using Domain.Models.ProductEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductRepository
    {

        //POST's METHODS
        Task<ProductResponse> AddCategoryAsync(CategoryDTO model);
        Task<ProductResponse> AddProductAsync(ProductDTO model);
        Task<ProductResponse> UpdateProductAsync(ProductDTO model);
        Task<ProductResponse> RemoveProductAsync(string productId);


        // GET's METHODS
        Task<List<ProductDTO>> GetProductBySeller(string userId);
        Task<IEnumerable<CategoryDTO>> GetAllCategories();        
        Task<IEnumerable<ProductDTO>> GetProducts();
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(string categoryTag);
        Task<IEnumerable<Product>> GetProductByIdAsync(string product);

    }
}

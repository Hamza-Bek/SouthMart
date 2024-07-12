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

        //Seller's methods
        Task<ProductResponse> AddProductAsync(ProductDTO model);
        Task<ProductResponse> UpdateProductAsync(ProductDTO model);
        Task<ProductResponse> RemoveProductAsync(string productId);

        //Buyer's methods
        Task<ProductResponse> AddProductToCartAsync(ProductDTO model);


        Task<List<ProductDTO>> GetProductBySeller(string userId);
    }
}

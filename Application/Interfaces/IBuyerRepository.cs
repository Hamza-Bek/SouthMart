using Application.DTOs.Request.ProductEntity;
using Application.DTOs.Response;
using Domain.Models.Authentication;
using Domain.Models.ProductEntity;
using Domain.Models.UserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBuyerRepository
    {
        event Action OnChange;

        Task<Cart> GetCartAsync(string userId);
        Task<IEnumerable<CartItem>> GetCartItemsAsync(string userId);

        Task<BuyerResponse> AddProductToCartAsync(string productId, string userId);
        Task<BuyerResponse> RemoveProductFromCartAsync(string productId, string userId);

        Task<BuyerResponse> LikeProductAsync(string productId, string userId);        
        Task<IEnumerable<ProductDTO>> GetLikedProductsAsync(string userId);
    }
}

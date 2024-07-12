using Application.DTOs.Response;
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
        Task<Cart> GetCartAsync(string userId);

        Task<BuyerResponse> AddProductToCartAsync(string productId, string userId);
        Task<BuyerResponse> RemoveProductFromCartAsync(string productId, string userId);
    }
}

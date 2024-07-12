using Application.DTOs.Request;
using Application.DTOs.Response;
using Domain.Models.SellerEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISellerRepository
    {
        Task<SellerResponse> CreateSellerAccountAsync(SellerAccountDTO model);
        Task<SellerAccountDTO> GetSellerAccountAsync(string userId);
    }
}

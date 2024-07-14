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

        // Total sales and products sold
        Task<SalesResponse> GetSalesForLastYearAsync(string sellerId);
        Task<SalesResponse> GetSalesForCurrentYearAsync(string sellerId);
        Task<SalesResponse> GetSalesForLastMonthAsync(string sellerId);
        Task<SalesResponse> GetSalesForCurrentMonthAsync(string sellerId);
        Task<SalesResponse> GetSalesForLast24HoursAsync(string sellerId);
    }
}

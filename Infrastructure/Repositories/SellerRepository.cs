using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Models.SellerEntity;
using Domain.Models.UserEntity;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SellerRepository(AppDbContext _context, IMapper _mapper) : ISellerRepository
    {
        public async Task<SellerResponse> CreateSellerAccountAsync(SellerAccountDTO model)
        {
            var map = _mapper.Map<SellerAccount>(model);

            var userId = model.SellerId;
            var user = await _context.Users.FindAsync(userId);
            
            if (user == null)
                return new SellerResponse(false, "User not found");

            if (model == null)
                return new SellerResponse(false, "Can not inster null values");

            var acc = new SellerAccount()
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Description = model.Description,
                DateCreated = DateTime.Now,
                GrossSales = 0,
                Revenue = 0,
                SellerId = userId,                
            };

            _context.SellerAccounts.Add(acc);

            await _context.SaveChangesAsync();

            return new SellerResponse(true, "Seller account request sent!");
        }

        public async Task<SalesResponse> GetSalesForCurrentMonthAsync(string sellerId)
        {
            try
            {
                var now = DateTime.Now;
                var startPeriod = new DateTime(now.Year, now.Month, 1);
                var endPeriod = startPeriod.AddMonths(1);

                Console.WriteLine($"Start Period: {startPeriod}, End Period: {endPeriod}");

                var lastMonthSales = await _context.OrderDetails
                    .Where(od => od.Order.SellerId == sellerId && od.Order.OrderDate >= startPeriod && od.Order.OrderDate < endPeriod)
                    .ToListAsync();

                var totalSales = lastMonthSales.Sum(od => od.ProductPrice * od.Quantity.GetValueOrDefault());

                return new SalesResponse(true, $"Sales data retrieved successfully, Start Period: {startPeriod}, End Period: {endPeriod}", lastMonthSales);
            }
            catch (Exception ex)
            {
                return new SalesResponse(false, $"Error: {ex.Message}", new List<OrderDetails>());
            }
        }

        public async Task<SalesResponse> GetSalesForLastMonthAsync(string sellerId)
        {
            try
            {
                var now = DateTime.Now;
                var startPeriod = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
                var endPeriod = new DateTime(now.Year, now.Month, 1);

                Console.WriteLine($"Start Period: {startPeriod}, End Period: {endPeriod}");

                var lastMonthSales = await _context.OrderDetails
                    .Where(od => od.Order.SellerId == sellerId && od.Order.OrderDate >= startPeriod && od.Order.OrderDate < endPeriod)
                    .ToListAsync();

                var totalSales = lastMonthSales.Sum(od => od.ProductPrice * od.Quantity.GetValueOrDefault());

                return new SalesResponse(true, $"Sales data retrieved successfully, Start Period: {startPeriod}, End Period: {endPeriod}", lastMonthSales);
            }
            catch (Exception ex)
            {
                return new SalesResponse(false, $"Error: {ex.Message}", new List<OrderDetails>());
            }
        }

        public async Task<SalesResponse> GetSalesForLast24HoursAsync(string sellerId)
        {
            try
            {
                var now = DateTime.Now;
                var startPeriod = now.AddHours(-24);
                var endPeriod = now;

                Console.WriteLine($"Start Period: {startPeriod}, End Period: {endPeriod}");

                var last24HoursSales = await _context.OrderDetails
                    .Where(od => od.Order.SellerId == sellerId && od.Order.OrderDate >= startPeriod && od.Order.OrderDate <= endPeriod)
                    .ToListAsync();

                var totalSales = last24HoursSales.Sum(od => od.ProductPrice * od.Quantity.GetValueOrDefault());

                return new SalesResponse(true, $"Sales data retrieved successfully, Start Period: {startPeriod}, End Period: {endPeriod}", last24HoursSales);
            }
            catch (Exception ex)
            {
                return new SalesResponse(false, $"Error: {ex.Message}", new List<OrderDetails>());
            }
        }

        public async Task<SellerAccountDTO> GetSellerAccountAsync(string userId)
        {            
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                Console.WriteLine("User not found");

            var getSellerAccount = await _context.SellerAccounts
                .FirstOrDefaultAsync(o => o.SellerId == userId);

            var sellerAccountDTO = _mapper.Map<SellerAccountDTO>(getSellerAccount);

            return sellerAccountDTO;
        }

        public async Task<SalesResponse> GetSalesForCurrentYearAsync(string sellerId)
        {
            try
            {
                var now = DateTime.Now;
                var startPeriod = new DateTime(now.Year, 1, 1);
                var endPeriod = new DateTime(now.Year + 1, 1, 1);

                Console.WriteLine($"Start Period: {startPeriod}, End Period: {endPeriod}");

                var currentYearSales = await _context.OrderDetails
                    .Where(od => od.Order.SellerId == sellerId && od.Order.OrderDate >= startPeriod && od.Order.OrderDate < endPeriod)
                    .ToListAsync();

                var totalSales = currentYearSales.Sum(od => od.ProductPrice * od.Quantity.GetValueOrDefault());

                return new SalesResponse(true, $"Sales data retrieved successfully, Start Period: {startPeriod}, End Period: {endPeriod}", currentYearSales);
            }
            catch (Exception ex)
            {
                return new SalesResponse(false, $"Error: {ex.Message}", new List<OrderDetails>());
            }
        }

        public async Task<SalesResponse> GetSalesForLastYearAsync(string sellerId)
        {
            try
            {
                var now = DateTime.Now;
                var startPeriod = new DateTime(now.Year - 1, 1, 1);
                var endPeriod = new DateTime(now.Year, 1, 1);

                Console.WriteLine($"Start Period: {startPeriod}, End Period: {endPeriod}");

                var currentYearSales = await _context.OrderDetails
                    .Where(od => od.Order.SellerId == sellerId && od.Order.OrderDate >= startPeriod && od.Order.OrderDate < endPeriod)
                    .ToListAsync();

                var totalSales = currentYearSales.Sum(od => od.ProductPrice * od.Quantity.GetValueOrDefault());

                return new SalesResponse(true, $"Sales data retrieved successfully, Start Period: {startPeriod}, End Period: {endPeriod}", currentYearSales);
            }
            catch (Exception ex)
            {
                return new SalesResponse(false, $"Error: {ex.Message}", new List<OrderDetails>());
            }
        }
    }
}

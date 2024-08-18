using Application.DTOs.Request;
using Application.DTOs.Request.ProductEntity;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Models.Authentication;
using Domain.Models.SellerEntity;
using Domain.Models.UserEntity;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class SellerRepository(AppDbContext _context, 
        IMapper _mapper, 
        UserManager<ApplicationUser> _userManager,
        RoleManager<IdentityRole> _roleManager,
        INotificationRepository _notificationRepository) : ISellerRepository
    {
        public async Task<SellerResponse> CreateSellerAccountAsync(SellerAccountDTO model)
        {
            try
            {
                var adminRole = await _roleManager.FindByNameAsync("Admin");
                if (adminRole == null)
                    throw new Exception("Admin role not found.");

                var adminUsers = await _userManager.GetUsersInRoleAsync(adminRole.Name);
                if (adminUsers == null || !adminUsers.Any())
                    throw new Exception("No users found with the Admin role.");

                var userIds = adminUsers.Select(u => u.Id).ToList();

                var user = await _context.Users.FindAsync(model.SellerId);
                if (user == null)
                    return new SellerResponse(false, "User not found");

                if (model == null)
                    return new SellerResponse(false, "Cannot insert null values");

                var acc = new SellerAccount
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    Description = model.Description,
                    DateCreated = DateTime.Now,
                    GrossSales = 0,
                    Revenue = 0,
                    SellerId = model.SellerId,
                    Status = "Pending"
                };

                _context.SellerAccounts.Add(acc);

                foreach (var adminId in userIds)
                {
                    await _notificationRepository.AddNotificationAsync(adminId, $"The user with id: {acc.SellerId} requested a seller account. Current Status: {acc.Status}");
                }

                await _context.SaveChangesAsync();

                return new SellerResponse(true, "Seller account request sent!");
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog)
                // For now, just output to console
                Console.WriteLine($"Exception: {ex.Message}");
                return new SellerResponse(false, $"Error : {ex.Message}");
            }

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
                    .Where(od => od.Order.OrderDate >= startPeriod && od.Order.OrderDate < endPeriod)
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
                    .Where(od => od.Order.OrderDate >= startPeriod && od.Order.OrderDate < endPeriod)
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
                    .Where(od => od.Order.OrderDate >= startPeriod && od.Order.OrderDate <= endPeriod)
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
                    .Where(od => od.Order.OrderDate >= startPeriod && od.Order.OrderDate < endPeriod)
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
                    .Where(od => od.Order.OrderDate >= startPeriod && od.Order.OrderDate < endPeriod)
                    .ToListAsync();

                var totalSales = currentYearSales.Sum(od => od.ProductPrice * od.Quantity.GetValueOrDefault());

                return new SalesResponse(true, $"Sales data retrieved successfully, Start Period: {startPeriod}, End Period: {endPeriod}", currentYearSales);
            }
            catch (Exception ex)
            {
                return new SalesResponse(false, $"Error: {ex.Message}", new List<OrderDetails>());
            }
        }

        public async Task<SellerResponse> ApproveSellerAccountAsync(string userId, bool isApproved)
        {
            var statuses = await _context.SellerStatuses.FirstOrDefaultAsync();

            var acc = await _context.SellerAccounts.FindAsync(userId);
            if (acc == null)
                return new SellerResponse(false, "Seller account not found");

            if (isApproved)
            {
                acc.Status = statuses.Approved;
                // Update the user's role to "Seller"
                var user = await _context.Users.FindAsync(acc.SellerId);
                if (user != null)
                {
                    // Assuming you have a method to change the user's role
                    await ChangeUserRoleToSeller(user);
                }
                await _notificationRepository.AddNotificationAsync(acc.SellerId, "Your seller account has been approved.");
            }
            else
            {
                acc.Status = statuses.Rejected;
                await _notificationRepository.AddNotificationAsync(acc.SellerId, "Your seller account has been rejected.");
            }

            await _context.SaveChangesAsync();
            return new SellerResponse(true, $"Seller account request {(isApproved ? "approved" : "rejected")}");
        }

        public async Task<SellerResponse> ChangeUserRoleToSeller(ApplicationUser user)
        {
            try
            {
                await _userManager.RemoveFromRoleAsync(user, "User");
                await _userManager.AddToRoleAsync(user, "Seller");

                return new SellerResponse(true, "User role changed to seller!");
            }
            catch(Exception ex)
            {
                return new SellerResponse(false, "Failed to give access to seller role!");
            }
          
        }        

        public async Task<SellerResponse> AddSellerStatus(SellerStatus model)
        {
            try
            {
                if (model == null)
                    return new SellerResponse(false, "Can't insert null values");

                var statuses = new SellerStatus()
                {
                    Approved = model.Approved,
                    Rejected = model.Rejected
                };

                _context.SellerStatuses.Add(statuses);
                await _context.SaveChangesAsync();

                return new SellerResponse(true, "Status Created!");
            }
            catch (Exception ex)
            {
                return new SellerResponse(false , $"Status failed to create! : {ex.Message}");
            }
            
        }

        public async Task<IEnumerable<ProductDTO>> GetExpiredProductAsync(string sellerId)
        {
            var seller = await _context.SellerAccounts.FirstOrDefaultAsync(i => i.SellerId == sellerId);
            if(seller == null)
                return Enumerable.Empty<ProductDTO>();

            var expiredProducts = await _context.Products
               .Where(p => p.SellerId == sellerId && p.Quantity == 0) // Filter for products with zero quantity
                .Include(i => i.Images) // Include related data if necessary
                  .ToListAsync();

            return _mapper.Map<IEnumerable<ProductDTO>>(expiredProducts);
        }

        public async Task<IEnumerable<ProductDTO>> GetRecentlyAddedProductAsync(string sellerId)
        {
            var seller = await _context.SellerAccounts.FirstOrDefaultAsync(i => i.SellerId == sellerId);
            if (seller == null)
                return Enumerable.Empty<ProductDTO>();

            var recentlyAddedProducts = await _context.Products
               .Where(p => p.SellerId == sellerId)
                 .OrderByDescending(p => p.AddedDate) 
                     .Take(10) 
                       .Include(i => i.Images)
                          .ToListAsync();
            
            return _mapper.Map<IEnumerable<ProductDTO>>(recentlyAddedProducts);
        }

        public async Task<IEnumerable<OrderDetails>> GetSellerOrders(string sellerId)
        {
            var seller = await _context.SellerAccounts.FirstOrDefaultAsync(i => i.SellerId == sellerId);
            if (seller == null)
                throw new Exception("Seller not found.");

            var products = await _context.OrderDetails
                .Where(i => i.SellerId == seller.SellerId)
                .Include(i => i.Order)      
                .ThenInclude(i => i.OrderMaker)
                     .ToListAsync();

            return _mapper.Map<IEnumerable<OrderDetails>>(products);
        }

        public async Task<IEnumerable<OrderDetails>> GetOrder(string orderId)
        {
            var orders = await _context.OrderDetails
                .Include(o => o.Order)       // Include related entities if needed
                .ThenInclude(o => o.OrderMaker) // Include related entities if needed
                .ThenInclude(l => l.Location)
                .Where(o => o.OrderId == orderId)
                .ToListAsync(); // Convert the result to a list

            return orders; // Return the list of orders
        }
    }
}

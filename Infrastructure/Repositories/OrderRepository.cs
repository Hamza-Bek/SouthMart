using Application.DTOs.Request.OrderEntity;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Models.UserEntity;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class OrderRepository(AppDbContext _context, IMapper _mapper) : IOrderRepository
    {
        public async Task<OrderResponse> PlaceOrderAsync(OrderDTO model)
        {
            try
            {
                var map = _mapper.Map<OrderDTO>(model);

                var userId = model.UserId;

                var user = await _context.Users
                    .Include(i => i.Location)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                    return new OrderResponse(false, "User not found");

                var userCart = await _context.Carts
                    .Include(p => p.CartItems)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (userCart == null)
                    return new OrderResponse(false, "Cart not found");

                var userLocation = await _context.Locations.FirstOrDefaultAsync(l => l.ApplicationUserId == userId);

                if (userLocation == null)
                    return new OrderResponse(false, "Location not found");

                var sellerId = model.SellerId;
                var seller = await _context.SellerAccounts.FirstOrDefaultAsync(s => s.SellerId == sellerId);
                if (seller == null)
                    return new OrderResponse(false, "Seller Id not found");

                decimal orderTotal = (decimal)userCart.CartTotal;

                userCart.CartTotal = 0;

                var order = new Order()
                {
                    OrderId = Guid.NewGuid().ToString(),
                    OrderNumber = GenerateRandomNumberString(5),
                    OrderDate = model.OrderDate,
                    OrderTotal = orderTotal,
                    OrderStatus = "Pending",
                    UserId = userId,      
                    SellerId = model.SellerId,
                    LocationId = userLocation.LocationId,
                    Location = userLocation,
                    OrderDetails = userCart.CartItems.Select(cartitem => new OrderDetails
                    {
                        OrderDetailId = Guid.NewGuid().ToString(),
                        OrderId = model.OrderId,
                        ProductName = cartitem.ProductName,
                        ProductPrice = cartitem.ProductPrice,
                        Quantity = cartitem.Quantity
                    }).ToList(),
                };

                _context.Entry(userLocation).State = EntityState.Unchanged;

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                return new OrderResponse(true, "Order placed successfully");
            }
            catch (DbUpdateException ex)
            {
                // Log the error (uncomment ex variable name and write a log.)
                var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new OrderResponse(false, $"An error occurred while saving the entity changes: {errorMessage}");
            }
            catch (Exception ex)
            {
                // Log the error (uncomment ex variable name and write a log.)
                return new OrderResponse(false, $"An unexpected error occurred: {ex.Message}");
            }
        }

        private string GenerateRandomNumberString(int length)
        {
            Random random = new Random();
            char[] digits = new char[length];

            for (int i = 0; i < length; i++)
            {
                digits[i] = (char)('0' + random.Next(0, 10));
            }

            return new string(digits);
        }
    }
}

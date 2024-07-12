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
        public async Task<OrderResponse> PlaceOrderAsync(OrderDTO model, string userId)
        {
            var map = _mapper.Map<OrderDTO>(model);

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

            decimal orderTotal = (decimal)userCart.CartTotal;

            var order = new Order()
            {
                OrderId = model.OrderId,
                OrderDate = model.OrderDate,
                OrderTotal = orderTotal,
                OrderStatus = "Pending",
                LocationId = userLocation.LocationId,
                Location = userLocation,
                OrderDetails = (ICollection<OrderDetails>)userCart.CartItems.Select(cartitem => new OrderDetails
                {
                    OrderDetailId = Guid.NewGuid().ToString(),
                    OrderId = model.OrderId,
                    ProductName = cartitem.ProductName,
                    ProductPrice = cartitem.ProductPrice,
                    Quantity = cartitem.Quantity
                }).ToList(),
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            throw new NotImplementedException();
        }
    }
}

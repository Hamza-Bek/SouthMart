using Application.DTOs.Request.OrderEntity;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Models.UserEntity;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<OrderResponse> ClearCartItemsAsync(string userId)
        {
            var getUser = await _context.Users.FindAsync(userId);
            if (getUser == null)
                return new OrderResponse(Flag: false, Message: "The user was not found");

            var getUserCart = await _context.Carts.Include(p => p.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
            if (getUserCart == null)
                return new OrderResponse(Flag: false, Message: "The user's cart is not found!");

            // Get all cart items for the user
            var getUserCartItems = _context.CartItems.Where(ci => ci.CartId == getUserCart.Id).ToList();
            if (!getUserCartItems.Any())
                return new OrderResponse(Flag: false, Message: "Cart is already clear");

            // Remove each cart item from the context
            _context.CartItems.RemoveRange(getUserCartItems);

            // Save changes to the database
            await _context.SaveChangesAsync();


            return new OrderResponse(Flag: true, Message: "The cart cleared!");
        }

        public async Task<OrderResponse> ClearCartTotalAsync(string userId)
        {
            // Retrieve the user from the database
            var getUser = await _context.Users.FindAsync(userId);
            if (getUser == null)
                return new OrderResponse(Flag: false, Message: "No user was found");

            // Retrieve the user's cart
            var getUserCart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (getUserCart == null)
                return new OrderResponse(Flag: false, Message: "No user's cart was found");

            // Clear the CartTotal
            getUserCart.CartTotal = 0;

            // Save the changes to the database
            await _context.SaveChangesAsync();

            return new OrderResponse(Flag: true, Message: "Cart Total cleared successfully");
        }

        public async Task<IEnumerable<OrderDTO>> GetUserOrdersAsync(string userId)
        {
            try
            {                
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                    return Enumerable.Empty<OrderDTO>();
                
                var orders = await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
                
                var orderDTOs = _mapper.Map<IEnumerable<OrderDTO>>(orders);

                return orderDTOs;
            }
            catch
            {
                // Handle any errors that occur
                return Enumerable.Empty<OrderDTO>(); // Return an empty list if an exception occurs
            }
        }

		public async Task<IEnumerable<Order>> GetUserOrdersByOrderIdAsync(string orderId)
		{
			var order = await _context.Orders
                .Include(o => o.OrderMaker)
                .Include(d => d.OrderDetails)
                .Include(l => l.Location)
                .FirstOrDefaultAsync(i => i.OrderId == orderId);

			if (order == null)
				return Enumerable.Empty<Order>();

			return new List<Order> { order };
		}

		public async Task<OrderResponse> PlaceOrderAsync(OrderDTO model)
        {
            try
            {
                var map = _mapper.Map<OrderDTO>(model);

                var userId = model.UserId;

                var user = await _context.Users
                    .Include(u => u.SellerAccount)
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

                userCart.CartTotal = 0;

                var order = new Order()
                {
                    OrderId = Guid.NewGuid().ToString(),
                    OrderNumber = GenerateRandomNumberString(5),
                    OrderDate = model.OrderDate,
                    OrderTotal = orderTotal,
                    OrderStatus = "Pending",
                    UserId = userId,                        
                    LocationId = userLocation.LocationId,
                    Location = userLocation,
                    OrderMaker = user,                    
					OrderDetails = userCart.CartItems.Select(cartItem => new OrderDetails
					{
						OrderDetailId = Guid.NewGuid().ToString(),
						ProductId = cartItem.ProductId,
						ProductName = cartItem.ProductName,
						ProductPrice = cartItem.ProductPrice,
						Quantity = cartItem.Quantity
					}).ToList()
				};           

                _context.Entry(userLocation).State = EntityState.Unchanged;
          

                foreach(var orderDetail in order.OrderDetails)
                {
                    var product = await _context.Products
                     .Include(p => p.Seller) // Include the Seller navigation property
                     .ThenInclude(s => s.SellerAccount) // Then include the SellerAccount
                     .FirstOrDefaultAsync(p => p.Id == orderDetail.ProductId);

                    if(product.Quantity == 0)
                        return new OrderResponse(false, "product not found");

                    if (product != null)
                    {
                        var sellerAccount = product.Seller?.SellerAccount;
                        if (sellerAccount != null)
                        {
                            product.Quantity -= orderDetail.Quantity;
                            sellerAccount.Revenue += (decimal)(product.SellingPrice - product.Cost);
                            sellerAccount.GrossSales += (decimal)(product.SellingPrice);
                            product.TotalSold = (product.TotalSold) + (orderDetail.Quantity ?? 0);
                            _context.Entry(product).State = EntityState.Modified;
                            _context.Entry(sellerAccount).State = EntityState.Modified;
                        }
                        else
                        {
                            // Handle the case where sellerAccount is null
                            // This might involve logging the issue or raising an exception
                        }
                    }
                }


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

using Application.DTOs.Request.Account;
using Application.DTOs.Request.ProductEntity;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Models.Authentication;
using Domain.Models.ProductEntity;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class NotificationRepository(AppDbContext _context , IMapper _mapper) : INotificationRepository
    {
        public async Task<NotificationResponse> AddNotificationAsync(string userId, string message)
        {
            var notification = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                Content = message,                
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return new NotificationResponse(true, "Notification added!");
        }

        public async Task<List<NotificationDTO>> GetAllNotifications(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                Console.WriteLine("User not found!");

            var notifications = await _context.Notifications
            .Where(u => u.UserId == userId)
            .ToListAsync();

            var notificationDTO = notifications.Select(p => _mapper.Map<NotificationDTO>(p)).ToList();

            return notificationDTO;
        }
        

        public async Task<NotificationResponse> LowQuantityNotifyAsync(NotificationDTO model)
        {
            _mapper.Map<NotificationDTO>(model);

            model.ThresholdQuantity = 2;

            var user = await _context.SellerAccounts.FirstOrDefaultAsync(u => u.SellerId == model.UserId);
            if (user == null)
                return new NotificationResponse(false, "Seller not found");

            var products = await _context.Products
                .Where(u => u.SellerId == user.SellerId)
                .ToListAsync();

            if (products == null)
                return new NotificationResponse(false, "No products in the seller's inventory");

            var notifications = new List<Notification>();

            foreach (var product in products)
            {
                if (product.Quantity <= model.ThresholdQuantity)
                {

                    var existingNotification = await _context.Notifications
                        .FirstOrDefaultAsync(n => n.ReferenceId == product.Id && n.ThresholdQuantity == model.ThresholdQuantity && n.UserId == model.UserId);


                    if (existingNotification == null)
                    {
                        var notification = new Notification
                        {
                            Id = Guid.NewGuid().ToString(),
                            Content = $"The product {product.Name} is below the threshold quantity of {model.ThresholdQuantity}.",
                            UserId = model.UserId,
                            User = user.Seller,
                            ReferenceId = product.Id,
                            ThresholdQuantity = model.ThresholdQuantity
                        };
                        notifications.Add(notification);
                    }
                }

            }

            if (!notifications.Any())
            {
                return new NotificationResponse(false, "No new notifications to send.");
            }

            _context.Notifications.AddRange(notifications);
            await _context.SaveChangesAsync();
            return new NotificationResponse(true, "Notification sent successfully");

        }
    
        public async Task<NotificationResponse> OrderPlacedNotify(NotificationDTO model)
        {
            try
            {
                _mapper.Map<NotificationDTO>(model);

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == model.UserId);
                if (user == null)
                    return new NotificationResponse(false, "User not found");

                var order = await _context.Orders
                    .Where(u => u.UserId == model.UserId)
                    .OrderByDescending(o => o.OrderDate)
                    .FirstOrDefaultAsync();

                if (order == null)
                    return new NotificationResponse(false, "No orders found for the user");

                var existingNotification = await _context.Notifications
                     .FirstOrDefaultAsync(n => n.ReferenceId == order.OrderId && n.UserId == model.UserId);

                if (existingNotification == null)
                {
                    var notification = new Notification
                    {
                        Id = Guid.NewGuid().ToString(),
                        Content = $"Your order #{order.OrderNumber} has been placed successfully.",
                        UserId = model.UserId,
                        User = user,
                        ReferenceId = order.OrderId, // Assuming this notification is not product-specific
                        ThresholdQuantity = 0 // Assuming this notification does not involve a threshold
                    };
                    _context.Notifications.Add(notification);
                    await _context.SaveChangesAsync();
                    return new NotificationResponse(true, "Notification sent successfully");
                }
                else
                {
                    return new NotificationResponse(false, "No new notifications to send.");
                }                            
            }
            catch (Exception ex)
            {
                return new NotificationResponse(false, $"Error : {ex.Message}");
            }
        }

    }
}

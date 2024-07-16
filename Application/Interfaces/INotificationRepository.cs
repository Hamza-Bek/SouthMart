﻿using Application.DTOs.Request.Account;
using Application.DTOs.Response;
using Domain.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface INotificationRepository
    {
        Task<List<NotificationDTO>> GetAllNotifications(string userId);

        //Buyer's methods :
        Task<NotificationResponse> OrderPlacedNotify(NotificationDTO model);

        //Seller's methods : 
        Task<NotificationResponse> LowQuantityNotifyAsync(NotificationDTO model);

       //Task<NotificationResponse> NewOrderPlacedNotifyAsync(NotificationDTO model);
    }
}

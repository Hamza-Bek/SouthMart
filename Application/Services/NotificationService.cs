using Application.DTOs.Request.Account;
using Application.DTOs.Request.OrderEntity;
using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class NotificationService (HttpClient _httpClient) : INotificationRepository
    {
        public async Task<NotificationResponse> AddNotificationAsync(string userId, string message)
        {
            var data = await _httpClient.PostAsJsonAsync("api/comments/add-comment", message);
            var response = await data.Content.ReadFromJsonAsync<NotificationResponse>();
            if (response.Flag)
            {
                return new NotificationResponse { Flag = true, Message = "Comment added successfully" };

            }
            else
            {

                return new NotificationResponse { Flag = false, Message = "Failed to add comment" };

            }
        }

        public async Task<IEnumerable<Notification>> GetAllNotifications()
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/notifications/get-notifications");
                string error = CheckResponseStatus(response);
                if (!string.IsNullOrEmpty(error))
                    throw new Exception(error);

                var result = await response.Content.ReadFromJsonAsync<IEnumerable<Notification>>();
                return result!;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task<NotificationResponse> LowQuantityNotifyAsync(NotificationDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<NotificationResponse> OrderPlacedNotify(NotificationDTO model)
        {
            throw new NotImplementedException();
        }
        private static string CheckResponseStatus(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return $"sorry unkown error occured.{Environment.NewLine} Error Description : {Environment.NewLine} Status Code : {response.StatusCode}{Environment.NewLine} Reason Phrase : {response.ReasonPhrase}";
            else
                return null!;
        }
    }
}

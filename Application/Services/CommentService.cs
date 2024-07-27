using Application.DTOs.Request.ProductEntity;
using Application.DTOs.Response;
using Application.Extensions;
using Application.Interfaces;
using Domain.Models.ProductEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CommentService : ICommentRepository
    {
        private readonly HttpClient _httpClient;
        private readonly HttpClientService httpClientService;
        public CommentService(HttpClient httpClient, HttpClientService httpClientService)
        {
            _httpClient = httpClient;
            this.httpClientService = httpClientService;
        }

        public async Task<NotificationResponse> AddCommentAsync(CommentDTO model)
        {
            try
            {
                var data = await _httpClient.PostAsJsonAsync("api/comments/add-comment", model);                
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
            catch (Exception ex)
            {
                // Log the exception (ex)
                return new NotificationResponse(false, $"Error : {ex.Message}");
            }
        }

        public Task<NotificationResponse> EditCommentAsync(CommentDTO model)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Comment>> GetProductCommentsAsync(string productId)
        {

            try
            {
                var response = await _httpClient.GetAsync($"api/comments/get-comments/{productId}");
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadFromJsonAsync<List<Comment>>();
                return data ?? new List<Comment>();
            }
            catch 
            {
                // Log the exception (ex)
                return new List<Comment>();
            }

        }
    }
}

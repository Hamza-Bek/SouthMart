using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Models.ProductEntity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FilesService (HttpClient _httpClient) : IFilesRepository
    {
        public async Task<GeneralResponse> DeleteImageAsync(string imageId)
        {
            var response = await _httpClient.DeleteAsync($"api/files/delete-image/{imageId}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<GeneralResponse>();
                return new GeneralResponse(Flag: false, Message: "Failed to remove the image");
            }

            var result = await response.Content.ReadFromJsonAsync<GeneralResponse>();
            return result!;
        }

        public async Task<ImageResponse> UploadImageAsync(IFormFile image, string plateId)
        {
            var formData = new MultipartFormDataContent();
            var streamContent = new StreamContent(image.OpenReadStream());
            formData.Add(streamContent, "file", image.FileName);

            //string requestUrl = $"api/files/upload-plate-image/{plateId}";
            HttpResponseMessage response = await _httpClient.PostAsync($"api/files/upload-plate-image/{plateId}", formData);

            if (!response.IsSuccessStatusCode)
            {
                return null!;
            }

            var result = await response.Content.ReadFromJsonAsync<ImageResponse>();
            return result!;
        }

        public Task<IEnumerable<Image>> GetAllImagesForProductAsync(string productId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Image> GetImageByIdAsync(string imageId)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse> SaveImageAsync(string productId, Image image)
        {
            throw new NotImplementedException();
        }

      
    }
}

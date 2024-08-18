using Application.DTOs.Response;
using Domain.Models;
using Domain.Models.ProductEntity;
using Microsoft.AspNetCore.Http;


namespace Application.Interfaces
{
    public interface IFilesRepository
    {
        Task<ImageResponse> UploadImageAsync(IFormFile image, string plateId);        
        Task<GeneralResponse> SaveImageAsync(string productId, Image image);
        Task<GeneralResponse> DeleteImageAsync(string imageId);
        Task<Image> GetImageByIdAsync(string imageId);
        Task<IEnumerable<Image>> GetAllImagesForProductAsync(string productId, CancellationToken cancellationToken);

    }
}

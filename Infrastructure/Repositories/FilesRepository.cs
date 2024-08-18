using Application.DTOs.Response;
using Application.Interfaces;
using Domain.Models.ProductEntity;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class FilesRepository (AppDbContext _context) : IFilesRepository
    {
      
        public async Task<GeneralResponse> DeleteImageAsync(string imageId)
        {
            var imageToDelete = await _context.Images.FindAsync(imageId);

            if (imageToDelete is not null)
            {
                File.Delete(imageToDelete.AbsolutePath!);

                var result = _context.Images.Remove(imageToDelete);

                if (result.State == EntityState.Deleted)
                {
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                return new GeneralResponse(Flag: false, Message: "The image doesn't exist in the system");
            }

            return new GeneralResponse(Flag: true, Message: "The image deleted!");
        }

        public async Task<GeneralResponse> SaveImageAsync(string productId, Image image)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
                return new GeneralResponse(false, "Product not found!");

            product?.Images.Add(image);
            await _context.SaveChangesAsync();

            return new GeneralResponse()
            {
                Flag = true,
                Message = "Image saved successfully"
            };
        }

        public Task<IEnumerable<Image>> GetAllImagesForProductAsync(string plateId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Image> GetImageByIdAsync(string imageId)
        {
            throw new NotImplementedException();
        }

        public Task<ImageResponse> UploadImageAsync(IFormFile image, string plateId)
        {
            throw new NotImplementedException();
        }

      
    }
}

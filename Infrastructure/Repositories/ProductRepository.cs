using Application.DTOs.Request.ProductEntity;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
using Domain.Models.ProductEntity;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Repositories
{
    public class ProductRepository(AppDbContext _context, IMapper _mapper , IFilesRepository _filesRepository) : IProductRepository
    {
        public async Task<ProductResponse> AddProductAsync(ProductDTO model)
        {
            try
            {
                var statuses = await _context.SellerStatuses.FirstOrDefaultAsync();

                var userId = model.SellerId;
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                    return new ProductResponse(false, "Seller id is empty");

                var sellerAccount = await _context.SellerAccounts
                      .SingleOrDefaultAsync(sa => sa.SellerId == userId);
                if (sellerAccount == null )
                {
                    Console.WriteLine($"SellerAccount not found for userId: {userId}");
                    return new ProductResponse(false, "User does not have access!");
                }

                if (sellerAccount.Status == statuses.Rejected)
                    return new ProductResponse(false, "Seller account is Rejected");

                if (userId != sellerAccount.SellerId)
                {
                    Console.WriteLine($"User ID mismatch: userId = {userId}, sellerAccount.Id = {sellerAccount.Id}");
                    return new ProductResponse(false, "User does not have access!");
                }

                if (model == null)
                    return new ProductResponse(false, "Can not insert null values");

                var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryTag == model.Category);
                if (category == null)
                    return new ProductResponse(false, "Category not found!");

                var pr = new Product()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Description = model.Description,
                    Category = model.Category,
                    Quantity = model.Quantity,
                    Cost = model.Cost,
                    SellingPrice = model.SellingPrice,
                    SellerId = userId,
                    AddedDate = DateTime.Now,
                };

                _context.Products.Add(pr);
                await _context.SaveChangesAsync();

                return new ProductResponse(true, "Product added Successfully!");
            }
            catch (Exception ex)
            {
                // Log the inner exception for more details
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }

                return new ProductResponse(false, "An error occurred while saving the entity changes. See the inner exception for details.");
            }

        }

        public async Task<ProductResponse> UpdateProductAsync(ProductDTO model)
        {
            try
            {
                if (model == null || string.IsNullOrEmpty(model.Id))
                    return new ProductResponse(false, "Can not insert null values");

                var product = await _context.Products.FindAsync(model.Id);
                if (product == null)
                    return new ProductResponse(false, "No produt found");

                var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == model.Category);
                if (category == null)
                    return new ProductResponse(false, "Category not found!");


                _mapper.Map(model, product);

                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return new ProductResponse(true, "Product updated successfully");
            }
            catch (Exception ex)
            {
                return new ProductResponse(false, $"Error : {ex.Message} ");
            }
        }

        public async Task<ProductResponse> RemoveProductAsync(string productId)
        {
            var product = await _context.Products
                .Include(i => i.Images)
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null || string.IsNullOrEmpty(productId))
                return new ProductResponse(false, "Product not found");

            if (product.Images is not null && product.Images.Any())
            {
                for (int i = 0; i < product.Images.Count; i++)
                {
                    await _filesRepository.DeleteImageAsync(product.Images.ElementAt(i).imageId);
                }
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return new ProductResponse(true, "Product Deleted!");
        }   

        public async Task<ProductResponse> AddCategoryAsync(CategoryDTO model)
        {
            
            if (model == null)
                return new ProductResponse(false, "Can't insert null values");

            var category = new Category()
            {
                CategoryId = Guid.NewGuid().ToString(),
                CategoryTag = model.CategoryTag
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return new ProductResponse(true, "Category added successfully");
        }
        
        public async Task<Dictionary<string, string>> GetCategoriesDicAsync()
        {
            var categories = await _context.Categories.AsNoTracking().ToListAsync();
            return categories.ToDictionary(c => c.CategoryId, c => c.CategoryTag);
        }
    }
}

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
    public class ProductRepository(AppDbContext _context, IMapper _mapper) : IProductRepository
    {
        public async Task<ProductResponse> AddProductAsync(ProductDTO model)
        {
            try
            {
                var userId = model.SellerId;
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                    return new ProductResponse(false, "Seller id is empty");

                var sellerAccount = await _context.SellerAccounts
                      .SingleOrDefaultAsync(sa => sa.SellerId == userId);
                if (sellerAccount == null)
                {
                    Console.WriteLine($"SellerAccount not found for userId: {userId}");
                    return new ProductResponse(false, "User does not have access!");
                }

                if (userId != sellerAccount.SellerId)
                {
                    Console.WriteLine($"User ID mismatch: userId = {userId}, sellerAccount.Id = {sellerAccount.Id}");
                    return new ProductResponse(false, "User does not have access!");
                }

                if (model == null)
                    return new ProductResponse(false, "Can not insert null values");

                var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == model.Category);
                if (category == null)
                    return new ProductResponse(false, "Category not found!");

                var pr = new Product()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    Description = model.Description,
                    Category = model.Category,
                    Quantity = model.Quantity,
                    Cost = model.Cost,
                    SellingPrice = model.SellingPrice,
                    SellerId = userId
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
            var product = await _context.Products.FindAsync(productId);

            if (product == null || string.IsNullOrEmpty(productId))
                return new ProductResponse(false, "Product not found");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return new ProductResponse(true, "Product Deleted!");
        }

        public async Task<List<ProductDTO>> GetProductBySeller(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                Console.WriteLine("User not found!");

            var products = await _context.Products
            .Where(p => p.SellerId == userId)
            .ToListAsync();

            var productDtos = products.Select(p => _mapper.Map<ProductDTO>(p)).ToList();

            return productDtos;
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

        public async Task<IEnumerable<CategoryDTO>> GetAllCategories() => (IEnumerable<CategoryDTO>)await _context.Categories.AsNoTracking().ToListAsync();
    }
}

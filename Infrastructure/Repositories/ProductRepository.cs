using Application.DTOs.Request.ProductEntity;
using Application.DTOs.Response;
using Application.Interfaces;
using AutoMapper;
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
    public class ProductRepository(AppDbContext _context , IMapper _mapper) : IProductRepository
    {
        public async Task<ProductResponse> AddProductAsync(ProductDTO model)
        {
            try
            {
                var userId = model.SellerId;
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                    return new ProductResponse(false, "Seller id is empty");

                if (model == null)
                    return new ProductResponse(false, "Can not insert null values");

                var pr = new Product()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    Description = model.Description,
                    Quantity = model.Quantity,
                    Price = model.Price,
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
            
            if(product == null || string.IsNullOrEmpty(productId))
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
    }
}

using Application.DTOs.Request.ProductEntity;
using Application.Interfaces;
using AutoMapper;
using Domain.Models.ProductEntity;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Repositories
{
    public class MenuRepository (AppDbContext _context , IMapper _mapper) : IMenuRepository
    {
        public async Task<IEnumerable<ProductDTO>> GetProductByIdAsync(string product)
        {            
            var getProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == product);
            if (getProduct == null)
                return Enumerable.Empty<ProductDTO>();

            var data = await _context.Products
                .Where(n => n.Name == getProduct.Name)
                .Include(i => i.Images)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProductDTO>>(data);
        }

        public async Task<IEnumerable<ProductDTO>> GetProductBySeller(string userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                Console.WriteLine("User not found!");

            var products = await _context.Products
            .Where(p => p.SellerId == userId)
            .Include(i => i.Images)
            .ToListAsync();

            var productDtos = products.Select(p => _mapper.Map<ProductDTO>(p)).ToList();

            return productDtos;
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategories() => (IEnumerable<CategoryDTO>)await _context.Categories.AsNoTracking().ToListAsync();

        public async Task<IEnumerable<ProductDTO>> GetProducts() => (IEnumerable<ProductDTO>)await _context.Products.AsNoTracking().ToListAsync();

        public async Task<IEnumerable<ProductDTO>> GetProductsByCategoryAsync(string categoryTag)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryTag == categoryTag);
            if (category == null)
                return Enumerable.Empty<ProductDTO>();

            var products = await _context.Products
                .Where(p => p.Category == category.CategoryTag)
            .Include(i => i.Images)
            .ToListAsync();

            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<IEnumerable<ProductDTO>> GetTopSellingProductsAsync()
        {
            // Step 1: Fetch the top-selling product IDs with their total sold counts
            var topSellingProductIds = await _context.Products
                .GroupBy(p => p.Id)
                .Select(group => new
                {
                    ProductId = group.Key,
                    TotalSold = group.Sum(p => p.TotalSold)
                })
                .OrderByDescending(g => g.TotalSold)
                .Take(2)
                .Select(g => g.ProductId) // Select only product IDs
                .ToListAsync();

            // Step 2: Fetch product details including images for the top-selling products
            var topSellingProducts = await _context.Products
                .Where(p => topSellingProductIds.Contains(p.Id))
                .Include(p => p.Images) // Ensure images are included
                .ToListAsync(); // Fetch all products and images

            // Map to ProductDTO
            var productDTOs = topSellingProducts.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Quantity = p.Quantity,
                Cost = p.Cost,
                SellingPrice = p.SellingPrice,
                SellerId = p.SellerId,
                TotalSold = p.TotalSold,
                CoverImageUrl = p.Images.FirstOrDefault()?.Url // Map image URL
            });

            return productDTOs;
        }

        public async Task<IEnumerable<ProductDTO>> GetNewestProductsAsync()
        {
            try
            {
                var products = await _context.Products
                    .OrderByDescending(d => d.AddedDate)
                    .Take(10)
                    .Include(i => i.Images)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<ProductDTO>>(products);
            }
            catch
            {
                return Enumerable.Empty<ProductDTO>();
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetRandomProductsAsync()
        {
            int numberOfRandomProducts = 10;

            var randomProducts = await _context.Products
                .Include(i => i.Images)
                .OrderBy(r => Guid.NewGuid()) // Use Guid.NewGuid() to randomize the order
                .Take(numberOfRandomProducts)
                .ToListAsync();

            var randomProductDTOs = _mapper.Map<IEnumerable<ProductDTO>>(randomProducts);

            return randomProductDTOs;
        }
    }
}

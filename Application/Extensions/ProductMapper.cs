using Application.DTOs.Request.ProductEntity;
using Domain.Models.ProductEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class ProductMapper
    {
        public static ProductDTO ToProductDTO(this Product product)
        {
            return new()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Quantity = product.Quantity,
                Cost = product.Cost,
                SellingPrice = product.SellingPrice,
                SellerId= product.SellerId,
                Category = product.Category,
                TotalSold = product.TotalSold,
                AddedDate = product.AddedDate,
                CoverImageUrl = product.Images.First().Url,
                
            };
        }
    }
}

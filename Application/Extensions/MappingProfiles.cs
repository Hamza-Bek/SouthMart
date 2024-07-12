using Application.DTOs.Request;
using Application.DTOs.Request.ProductEntity;
using AutoMapper;
using Domain.Models.ProductEntity;
using Domain.Models.SellerEntity;

namespace Application.Extensions
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<SellerAccountDTO, SellerAccount>();           
            CreateMap<SellerAccount, SellerAccountDTO>();    
            
            CreateMap<Inventory, InventoryDTO>();
            CreateMap<InventoryDTO, Inventory>();

            CreateMap<ProductDTO, Product>();
            CreateMap<Product, ProductDTO>();
        }

    }
}

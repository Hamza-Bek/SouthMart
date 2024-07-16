using Application.DTOs.Request;
using Application.DTOs.Request.Account;
using Application.DTOs.Request.OrderEntity;
using Application.DTOs.Request.ProductEntity;
using AutoMapper;
using Domain.Models.Authentication;
using Domain.Models.ProductEntity;
using Domain.Models.SellerEntity;
using Domain.Models.UserEntity;

namespace Application.Extensions
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<SellerAccountDTO, SellerAccount>();           
            CreateMap<SellerAccount, SellerAccountDTO>();                        

            CreateMap<ProductDTO, Product>();
            CreateMap<Product, ProductDTO>();

            CreateMap<Order, OrderDTO>();
            CreateMap<Order, OrderDTO>();

            CreateMap<LocationDTO, Location>(); 
            CreateMap<Location, LocationDTO>();

            CreateMap<NotificationDTO, Notification>();
            CreateMap<Notification, NotificationDTO>();

            CreateMap<CommentDTO, Comment>();
            CreateMap<Comment, CommentDTO>();
        }

    }
}

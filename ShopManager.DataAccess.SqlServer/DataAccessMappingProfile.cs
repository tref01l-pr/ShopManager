using AutoMapper;
using ShopManager.DataAccess.SqlServer.Entities;
using ShopManager.Domain.Dtos.CategoryDtos;
using ShopManager.Domain.Dtos.OrderDtos;
using ShopManager.Domain.Dtos.OrderItemDtos;
using ShopManager.Domain.Dtos.ProductDtos;
using ShopManager.Domain.Dtos.UserDtos;
using ShopManager.Domain.Models;

namespace ShopManager.DataAccess.SqlServer;

public class DataAccessMappingProfile : Profile
{
    public DataAccessMappingProfile()
    {
        CreateMap<UserEntity, User>().ReverseMap();
        CreateMap<UserEntity, UserEntity>();
        CreateMap<UserEntity, UserDto>();
        CreateMap<UserEntity, UserDtoOrders>();
        
        CreateMap<SessionEntity, Session>().ReverseMap();

        CreateMap<CategoryEntity, Category>().ReverseMap();
        CreateMap<CategoryEntity, CategoryDto>().ReverseMap();

        CreateMap<ProductEntity, Product>().ReverseMap();
        CreateMap<ProductEntity, ProductDto>();

        CreateMap<OrderEntity, Order>().ReverseMap();
        CreateMap<OrderEntity, OrderDto>();
        
        CreateMap<OrderEntity, OrderDtoOrderItems>()
            .ForMember(
                dst => dst.OrderItems,
                opt => opt.MapFrom(src => src.OrderItems));
        
        CreateMap<OrderEntity, OrderDtoUser>()
            .ForMember(
                dst => dst.User,
                opt => opt.MapFrom(src => src.User));

        CreateMap<OrderItemEntity, OrderItem>().ReverseMap();
        CreateMap<OrderItemEntity, OrderItemDto>();
        CreateMap<OrderItemEntity, OrderItemDtoProduct>()
            .ForMember(
                dst => dst.Product,
                opt => opt.MapFrom(src => src.Product));
        
        

    }
}
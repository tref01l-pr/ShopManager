namespace ShopManager.API;

using AutoMapper;
using ShopManager.API.Contracts.Requests;
using ShopManager.API.Contracts.Responses;
using ShopManager.Domain.Dtos.CategoryDtos;
using ShopManager.Domain.Dtos.OrderDtos;
using ShopManager.Domain.Dtos.OrderItemDtos;
using ShopManager.Domain.Dtos.ProductDtos;
using ShopManager.Domain.Dtos.UserDtos;
using ShopManager.Domain.Models;

public class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<User, GetUserResponse>();
        CreateMap<UserDto, UserBirthdayResponse>()
            .ForMember(
                dst => dst.FullName,
                opt => opt.MapFrom(ud => ud.FirstName + " " + ud.MiddleName + " " + ud.LastName));

        CreateMap<RecentUserDto, RecentOrderResponse>()
            .ForMember(
                dst => dst.FullName,
                opt => opt.MapFrom(od => od.FirstName + " " + od.MiddleName + " " + od.LastName));

        CreateMap<OrderItemRequest, OrderItemDto>();
        CreateMap<OrderItemDtoProduct, OrderItemWithProductResponse>().ReverseMap();

        CreateMap<OrderDtoOrderItems, OrderResponse>().ReverseMap();

        CreateMap<ProductDto, ProductResponse>().ReverseMap();
        
        CreateMap<UserCategoryDto, UserCategoryResponse>().ReverseMap();
    }
}
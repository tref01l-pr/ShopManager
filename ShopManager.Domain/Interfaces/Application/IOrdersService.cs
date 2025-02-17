using CSharpFunctionalExtensions;
using ShopManager.Domain.Dtos.CategoryDtos;
using ShopManager.Domain.Dtos.OrderItemDtos;
using ShopManager.Domain.Dtos.UserDtos;
using ShopManager.Domain.Interfaces.BaseInterfaces;
using ShopManager.Domain.Models;

namespace ShopManager.Domain.Interfaces;

public interface IOrdersService : ICrudService<Order, int>
{
    Task<Result<TProjectTo>> CreateAsync<TProjectTo>(List<OrderItemDto> createOrderRequest, Guid userId);
    Task<Result<List<RecentUserDto>>> GetRecentOrdersAsync(int days);
}
using ShopManager.Domain.Dtos.UserDtos;
using ShopManager.Domain.Interfaces.BaseInterfaces;
using ShopManager.Domain.Models;

namespace ShopManager.Domain.Interfaces.Repositories;

public interface IOrdersRepository : ICrudRepository<Order, int>
{
    Task<List<RecentUserDto>> GetRecentUsersAsync(int days);
}
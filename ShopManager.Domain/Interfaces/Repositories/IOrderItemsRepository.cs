using ShopManager.Domain.Dtos.CategoryDtos;
using ShopManager.Domain.Interfaces.BaseInterfaces;
using ShopManager.Domain.Models;

namespace ShopManager.Domain.Interfaces.Repositories;

public interface IOrderItemsRepository : ICrudRepository<OrderItem, int>
{
    Task<List<UserCategoryDto>> GetUserCategoriesAsync(Guid userId);
}
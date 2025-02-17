using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopManager.DataAccess.SqlServer.Entities;
using ShopManager.DataAccess.SqlServer.Repositories.BaseRepositories;
using ShopManager.Domain.Dtos.CategoryDtos;
using ShopManager.Domain.Interfaces.Repositories;
using ShopManager.Domain.Models;

namespace ShopManager.DataAccess.SqlServer.Repositories;

public class OrderItemsRepository : BaseCrudRepository<ShopManagerDbContext, OrderItemEntity, OrderItem, int>,
    IOrderItemsRepository
{
    public OrderItemsRepository(ShopManagerDbContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    public async Task<List<UserCategoryDto>> GetUserCategoriesAsync(Guid userId)
    {
        var categories = await _context.OrderItems
            .AsNoTracking()
            .Where(oi => oi.Order.UserId == userId)
            .GroupBy(oi => oi.Product.Category)
            .Select(g => new UserCategoryDto
            {
                Id = g.Key.Id,
                Name = g.Key.Name,
                TotalQuantity = g.Sum(oi => oi.Quantity),
            })
            .OrderByDescending(x => x.TotalQuantity)
            .ToListAsync();

        return categories;
    }
}
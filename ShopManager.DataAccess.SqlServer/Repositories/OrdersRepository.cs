using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShopManager.DataAccess.SqlServer.Entities;
using ShopManager.DataAccess.SqlServer.Repositories.BaseRepositories;
using ShopManager.Domain.Dtos.UserDtos;
using ShopManager.Domain.Interfaces.Repositories;
using ShopManager.Domain.Models;

namespace ShopManager.DataAccess.SqlServer.Repositories;

public class OrdersRepository : BaseCrudRepository<ShopManagerDbContext, OrderEntity, Order, int>,
    IOrdersRepository
{
    public OrdersRepository(ShopManagerDbContext context, IMapper mapper) : base(context, mapper)
    {
    }

    public async Task<List<RecentUserDto>> GetRecentUsersAsync(int days)
    {
        var dateThreshold = DateTime.UtcNow.AddDays(-days);

        var recentUsers = await _context.Orders
            .AsNoTracking()
            .Where(o => o.OrderDate >= dateThreshold)
            .GroupBy(o => o.User)
            .Select(g => new RecentUserDto
            {
                Id = g.Key.Id,
                FirstName = g.Key.FirstName,
                MiddleName = g.Key.MiddleName,
                LastName = g.Key.LastName,
                LastOrderDate = g.Max(o => o.OrderDate)
            })
            .ToListAsync();

        return recentUsers;
    }
}
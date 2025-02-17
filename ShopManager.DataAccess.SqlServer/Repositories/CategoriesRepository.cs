using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShopManager.DataAccess.SqlServer.Entities;
using ShopManager.DataAccess.SqlServer.Repositories.BaseRepositories;
using ShopManager.Domain.Interfaces.Repositories;
using ShopManager.Domain.Models;

namespace ShopManager.DataAccess.SqlServer.Repositories;

public class CategoriesRepository : BaseCrudRepository<ShopManagerDbContext, CategoryEntity, Category, int>,
    ICategoriesRepository
{
    public CategoriesRepository(ShopManagerDbContext context, IMapper mapper)
        : base(context, mapper)
    {
    }

    public async Task<TProjectTo?> GetByNameAsync<TProjectTo>(string modelName) =>
        await _context.Categories
            .AsNoTracking()
            .Where(c => c.Name == modelName)
            .ProjectTo<TProjectTo>(_mapperConfig)
            .FirstOrDefaultAsync();

    public async Task<IList<TProjectTo>> GetAllAsync<TProjectTo>() =>
        await _context.Categories
            .AsNoTracking()
            .ProjectTo<TProjectTo>(_mapperConfig)
            .ToListAsync();
}
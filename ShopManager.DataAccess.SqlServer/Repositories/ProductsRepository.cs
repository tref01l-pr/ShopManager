using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ShopManager.DataAccess.SqlServer.Entities;
using ShopManager.DataAccess.SqlServer.Repositories.BaseRepositories;
using ShopManager.Domain.Interfaces.Repositories;
using ShopManager.Domain.Models;

namespace ShopManager.DataAccess.SqlServer.Repositories;

public class ProductsRepository : BaseCrudRepository<ShopManagerDbContext, ProductEntity, Product, int>,
    IProductsRepository
{
    public ProductsRepository(ShopManagerDbContext context, IMapper mapper)
        : base(context, mapper)
    {
    }


    public async Task<IList<TProjectTo>> GetAllAsync<TProjectTo>() =>
        await _context.Products
            .AsNoTracking()
            .ProjectTo<TProjectTo>(_mapperConfig)
            .ToListAsync();
}
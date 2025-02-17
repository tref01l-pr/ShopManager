using ShopManager.Domain.Interfaces.BaseInterfaces;
using ShopManager.Domain.Models;

namespace ShopManager.Domain.Interfaces.Repositories;

public interface IProductsRepository : ICrudRepository<Product, int>
{
    Task<IList<TProjectTo>> GetAllAsync<TProjectTo>();
}
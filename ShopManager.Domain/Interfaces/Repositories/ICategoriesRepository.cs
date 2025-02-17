using ShopManager.Domain.Interfaces.BaseInterfaces;
using ShopManager.Domain.Models;

namespace ShopManager.Domain.Interfaces.Repositories;

public interface ICategoriesRepository : ICrudRepository<Category, int>
{
    Task<TProjectTo?> GetByNameAsync<TProjectTo>(string modelName);
    Task<IList<TProjectTo>> GetAllAsync<TProjectTo>();
}
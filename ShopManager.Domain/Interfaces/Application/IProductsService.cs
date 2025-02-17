using CSharpFunctionalExtensions;
using ShopManager.Domain.Interfaces.BaseInterfaces;
using ShopManager.Domain.Models;

namespace ShopManager.Domain.Interfaces;

public interface IProductsService : ICrudService<Product, int>
{
    Task<Result<IList<TProjectTo>>> GetAllAsync<TProjectTo>();
}
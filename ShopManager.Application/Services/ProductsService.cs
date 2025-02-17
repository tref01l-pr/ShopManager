using CSharpFunctionalExtensions;
using ShopManager.Application.Services.BaseServices;
using ShopManager.DataAccess.SqlServer.Entities;
using ShopManager.Domain.Interfaces;
using ShopManager.Domain.Interfaces.Repositories;
using ShopManager.Domain.Models;

namespace ShopManager.Application.Services;

public class ProductsService : CrudService<IProductsRepository, ProductEntity, Product, int>, IProductsService
{
    public ProductsService(IProductsRepository repository, ITransactionsRepository transactionsRepository)
        : base(repository, transactionsRepository)
    {
    }
    
    public async Task<Result<IList<TProjectTo>>> GetAllAsync<TProjectTo>()
    {
        try
        {
            var products = await _repository.GetAllAsync<TProjectTo>();

            return Result.Success(products);
        }
        catch (Exception e)
        {
            return Result.Failure<IList<TProjectTo>>($"Error getting company by id: {e.Message}");
        }
    }
}
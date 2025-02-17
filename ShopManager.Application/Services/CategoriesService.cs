using CSharpFunctionalExtensions;
using ShopManager.Application.Services.BaseServices;
using ShopManager.DataAccess.SqlServer.Entities;
using ShopManager.Domain.Dtos.CategoryDtos;
using ShopManager.Domain.Interfaces;
using ShopManager.Domain.Interfaces.Repositories;
using ShopManager.Domain.Models;

namespace ShopManager.Application.Services;

public class CategoriesService : CrudService<ICategoriesRepository, CategoryEntity, Category, int>, ICategoriesService
{
    private readonly IOrderItemsRepository _orderItemsRepository;

    public CategoriesService(ICategoriesRepository repository, ITransactionsRepository transactionsRepository, IOrderItemsRepository orderItemsRepository)
        : base(
        repository, transactionsRepository)
    {
        _orderItemsRepository = orderItemsRepository;
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
            return Result.Failure<IList<TProjectTo>>($"Error getting all categories: {e.Message}");
        }
    }
    
    public override async Task<Result<TProjectTo>> CreateAsync<TProjectTo>(Category model)
    {
        if ((await _repository.GetByNameAsync<Category>(model.Name)) != null)
        {
            return Result.Failure<TProjectTo>("Category with this name already exists");
        }

        return await base.CreateAsync<TProjectTo>(model);
    }
    
    public async Task<Result<List<UserCategoryDto>>> GetUserCategoriesAsync(Guid userId)
    {
        try
        {
            var users = await _orderItemsRepository.GetUserCategoriesAsync(userId);

            return users;
        }
        catch (Exception e)
        {
            return Result.Failure<List<UserCategoryDto>>(e.Message);
        }
    }
}
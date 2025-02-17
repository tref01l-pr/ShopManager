using CSharpFunctionalExtensions;
using ShopManager.Domain.Dtos.CategoryDtos;
using ShopManager.Domain.Interfaces.BaseInterfaces;
using ShopManager.Domain.Models;

namespace ShopManager.Domain.Interfaces;

public interface ICategoriesService : ICrudService<Category, int>
{
    Task<Result<IList<TProjectTo>>> GetAllAsync<TProjectTo>();
    Task<Result<List<UserCategoryDto>>> GetUserCategoriesAsync(Guid userId);
}
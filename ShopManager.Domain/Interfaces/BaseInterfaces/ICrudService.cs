using CSharpFunctionalExtensions;

namespace ShopManager.Domain.Interfaces.BaseInterfaces;

public interface ICrudService<TModel, TKey>
{
    Task<Result<TProjectTo?>> GetByIdAsync<TProjectTo>(TKey id);
    Task<Result<TProjectTo>> CreateAsync<TProjectTo>(TModel model);
    Task<Result<TProjectTo>> UpdateAsync<TProjectTo>(TModel model);

    Task<Result> DeleteByIdAsync(TKey id);
}
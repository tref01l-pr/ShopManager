using CSharpFunctionalExtensions;

namespace ShopManager.Domain.Interfaces.BaseInterfaces;

public interface ICrudRepository<TModel, TKey>
{
    Task<TProjectTo?> GetByIdAsync<TProjectTo>(TKey id);
    Task<Result<TProjectTo>> CreateAsync<TProjectTo>(TModel model);
    Task<Result<TProjectTo>> UpdateAsync<TProjectTo>(TModel model);

    Task<Result> DeleteByIdAsync(TKey id);
}
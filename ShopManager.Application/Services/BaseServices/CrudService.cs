using CSharpFunctionalExtensions;
using ShopManager.Domain.Interfaces.BaseInterfaces;
using ShopManager.Domain.Interfaces.Repositories;

namespace ShopManager.Application.Services.BaseServices;

public abstract class CrudService<TRepository, TEntity, TModel, TKey> : ICrudService<TModel, TKey> 
    where TRepository : ICrudRepository<TModel, TKey>
{
    protected readonly ITransactionsRepository _transactionsRepository;
    protected readonly TRepository _repository;
    
    public CrudService(TRepository repository,ITransactionsRepository transactionsRepository)
    {
        _transactionsRepository = transactionsRepository;
        _repository = repository;
    }
    
    
    public async Task<Result<TProjectTo?>> GetByIdAsync<TProjectTo>(TKey id)
    {
        try
        {
            return await _repository.GetByIdAsync<TProjectTo>(id);
        }
        catch (Exception e)
        {
            return Result.Failure<TProjectTo?>(e.Message);
        }
    }

    public virtual async Task<Result<TProjectTo>> CreateAsync<TProjectTo>(TModel model)
    {
        var transaction = await _transactionsRepository.BeginTransactionAsync();
        try
        {
            var result = await _repository.CreateAsync<TProjectTo>(model);
            if (result.IsFailure)
            {
                throw new Exception(result.Error);
            }

            await _transactionsRepository.CommitTransactionAsync(transaction);
            return result.Value;
        }
        catch (Exception e)
        {
            await _transactionsRepository.RollbackTransactionAsync(transaction);
            return Result.Failure<TProjectTo>(e.Message);
        }
    }

    public virtual async Task<Result<TProjectTo>> UpdateAsync<TProjectTo>(TModel model)
    {
        await using var transaction = await _transactionsRepository.BeginTransactionAsync();
        try
        {
            var result = await _repository.UpdateAsync<TProjectTo>(model);
            if (result.IsFailure)
            {
                throw new Exception(result.Error);
            }
            await _transactionsRepository.CommitTransactionAsync(transaction);
            return result.Value;
        }
        catch (Exception e)
        {
            await _transactionsRepository.RollbackTransactionAsync(transaction);
            return Result.Failure<TProjectTo>(e.Message);
        }
    }

    public virtual async Task<Result> DeleteByIdAsync(TKey id)
    {
        await using var transaction = await _transactionsRepository.BeginTransactionAsync();
        try
        {
            var result = await _repository.DeleteByIdAsync(id);
            if (result.IsFailure)
            {
                throw new Exception(result.Error);
            }

            await _transactionsRepository.CommitTransactionAsync(transaction);
            return Result.Success();
        }
        catch (Exception e)
        {
            await _transactionsRepository.RollbackTransactionAsync(transaction);
            return Result.Failure(e.Message);
        }
    }
}
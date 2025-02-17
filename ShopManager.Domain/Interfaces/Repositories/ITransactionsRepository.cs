using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore.Storage;

namespace ShopManager.Domain.Interfaces.Repositories;

public interface ITransactionsRepository
{
    Task<IDbContextTransaction> BeginTransactionAsync(bool keepTransaction = true);

    Task<Result> CommitTransactionAsync(IDbContextTransaction transaction);
    Task<Result> RollbackTransactionAsync(IDbContextTransaction transaction);
}
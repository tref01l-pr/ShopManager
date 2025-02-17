using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore.Storage;
using ShopManager.Domain.Interfaces.Repositories;

namespace ShopManager.DataAccess.SqlServer.Repositories;

public class TransactionsRepository : ITransactionsRepository
{
    private readonly ShopManagerDbContext _context;
    private IDbContextTransaction? _transaction = null;
    
    public TransactionsRepository(ShopManagerDbContext context)
    {
        _context = context;
    }
    
    public async Task<IDbContextTransaction> BeginTransactionAsync(bool keepTransaction = true)
    {
        if (keepTransaction && _transaction != null)
        {
            return _transaction;
        }

        return _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task<Result> RollbackTransactionAsync(IDbContextTransaction transaction)
    {
        if (_transaction == null)
        {
            return Result.Failure("Transaction not opened");
        }

        if (transaction != _transaction)
        {
            return Result.Failure("Transaction not equal to private _transaction");
        }

        await _transaction.RollbackAsync();
        _transaction = null;
        return Result.Success();
    }

    public async Task<Result> CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (_transaction == null)
        {
            return Result.Failure("Transaction not opened");
        }

        if (transaction != _transaction)
        {
            return Result.Failure("Transaction not equal to private _transaction");
        }

        await _transaction.CommitAsync();
        _transaction = null;
        return Result.Success();
    }
}
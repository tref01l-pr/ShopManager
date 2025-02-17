using System.Linq.Expressions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ShopManager.Domain.Interfaces;
using ShopManager.Domain.Interfaces.BaseInterfaces;
using ShopManager.Domain.Interfaces.Repositories;

namespace ShopManager.DataAccess.SqlServer.Repositories.BaseRepositories;

public abstract class BaseCrudRepository<TContext, TEntity, TModel, TKey>
    : ICrudRepository<TModel, TKey>
    where TContext : DbContext
    where TEntity : class, IDbKey<TKey>
    where TModel : class, IModelKey<TKey>
{
    protected BaseCrudRepository(TContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    protected IConfigurationProvider _mapperConfig => _mapper.ConfigurationProvider;

    protected readonly TContext _context;
    private readonly IMapper _mapper;

    public async Task<TProjectTo?> GetByIdAsync<TProjectTo>(TKey id) =>
        await _context.Set<TEntity>()
            .Where(e => e.Id!.Equals(id))
            .ProjectTo<TProjectTo>(_mapperConfig)
            .FirstOrDefaultAsync();

    public virtual async Task<Result<TProjectTo>> CreateAsync<TProjectTo>(TModel model)
    {
        try
        {
            var entity = await _context.Set<TEntity>().AddAsync(_mapper.Map<TModel, TEntity>(model));
            var result = await SaveAsync(_context);

            if (!result.Value)
            {
                return Result.Failure<TProjectTo>($"Something went wrong during create {typeof(TEntity)}");
            }

            var response = await _context.Set<TEntity>()
                .Where(e => e.Id!.Equals(entity.Entity.Id))
                .ProjectTo<TProjectTo>(_mapperConfig)
                .FirstOrDefaultAsync();

            return response ?? Result.Failure<TProjectTo>($"{typeof(TEntity)} with that id not found!");
        }
        catch (Exception e)
        {
            return Result.Failure<TProjectTo>(e.Message);
        }
    }

    public virtual async Task<Result<TProjectTo>> UpdateAsync<TProjectTo>(TModel model)
    {
        try
        {
            var entity = await _context.Set<TEntity>().FindAsync(model.Id);
            if (entity == null)
            {
                return Result.Failure<TProjectTo>($"{typeof(TEntity)} not found");
            }

            _context.Entry(entity).CurrentValues.SetValues(model);

            var result = await SaveAsync(_context);

            if (!result.Value)
            {
                return Result.Failure<TProjectTo>($"Something went wrong during create {typeof(TEntity)}");
            }

            var response = await _context.Set<TEntity>()
                .Where(e => e.Id!.Equals(model.Id))
                .ProjectTo<TProjectTo>(_mapperConfig)
                .FirstOrDefaultAsync();

            return response ?? Result.Failure<TProjectTo>($"{typeof(TEntity)} with that id not found!");
        }
        catch (Exception e)
        {
            return Result.Failure<TProjectTo>(e.Message);
        }
    }

    public virtual async Task<Result> DeleteByIdAsync(TKey id) =>
        await DeleteAsync(x => x.Id!.Equals(id));

    protected virtual async Task<Result> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entities = await _context.Set<TEntity>()
                .Where(predicate)
                .ToArrayAsync();
            if (!entities.Any())
            {
                return Result.Failure($"{typeof(TEntity)} not found");
            }

            _context.Set<TEntity>().RemoveRange(entities);
            var result = await SaveAsync(_context);

            return result.Value
                ? Result.Success()
                : Result.Failure($"Something went wrong during deletion of {typeof(TEntity)}!");
        }
        catch (Exception e)
        {
            return Result.Failure(e.Message);
        }
    }

    protected async Task<Result<bool>> SaveAsync(TContext context) => await context.SaveChangesAsync() > 0;
}
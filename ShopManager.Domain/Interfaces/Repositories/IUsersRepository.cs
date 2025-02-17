using CSharpFunctionalExtensions;
using ShopManager.Domain.Models;

namespace ShopManager.Domain.Interfaces.Repositories;

public interface IUsersRepository
{
    Task<TProjectTo?> GetByIdAsync<TProjectTo>(Guid id) where TProjectTo : class;
    Task<User[]> GetByIdsAsync(IEnumerable<Guid> userIds);
    Task<TProjectTo?> GetByEmailAsync<TProjectTo>(string email) where TProjectTo : class;
    Task<Result<List<TProjectTo>>> GetBirthdayUsersAsync<TProjectTo>(int day, int month);
    Task<User[]> GetAsync();
    Task<bool> Delete(Guid id);
}
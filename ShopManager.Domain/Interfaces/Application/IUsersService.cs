using CSharpFunctionalExtensions;

namespace ShopManager.Domain.Interfaces;

public interface IUsersService
{
    Task<Result<TProjectTo?>> GetByEmailAndCompanyIdAsync<TProjectTo>(string email) where TProjectTo : class;
    Task<Result<TProjectTo?>> GetById<TProjectTo>(Guid id) where TProjectTo : class;
    Task<Result<List<TProjectTo>>> GetBirthdayUsersAsync<TProjectTo>(DateOnly date);
}
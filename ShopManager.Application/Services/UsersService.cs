using CSharpFunctionalExtensions;
using ShopManager.Domain.Interfaces;
using ShopManager.Domain.Interfaces.Repositories;

namespace ShopManager.Application.Services;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;

    public UsersService(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }
    
    public async Task<Result<TProjectTo?>> GetByEmailAndCompanyIdAsync<TProjectTo>(string email) where TProjectTo : class
    {
        try
        {
            var user = await _usersRepository.GetByEmailAsync<TProjectTo>(email);

            return user;
        }
        catch (Exception e)
        {
            return Result.Failure<TProjectTo?>(e.Message);
        }
    }

    public async Task<Result<TProjectTo?>> GetById<TProjectTo>(Guid id) where TProjectTo : class
    {
        try
        {
            var user = await _usersRepository.GetByIdAsync<TProjectTo>(id);

            return user;
        }
        catch (Exception e)
        {
            return Result.Failure<TProjectTo?>(e.Message);
        }
    }

    public async Task<Result<List<TProjectTo>>> GetBirthdayUsersAsync<TProjectTo>(DateOnly date)
    {
        try
        {
            var users = await _usersRepository.GetBirthdayUsersAsync<TProjectTo>(date.Day, date.Month);

            return users;
        }
        catch (Exception e)
        {
            return Result.Failure<List<TProjectTo>>(e.Message);
        }
    }
}
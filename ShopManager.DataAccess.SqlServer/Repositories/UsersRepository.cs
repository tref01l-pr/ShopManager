using AutoMapper;
using AutoMapper.QueryableExtensions;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ShopManager.DataAccess.SqlServer.Entities;
using ShopManager.Domain.Interfaces.Repositories;
using ShopManager.Domain.Models;

namespace ShopManager.DataAccess.SqlServer.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly ShopManagerDbContext _context;
    private readonly IMapper _mapper;

    private IConfigurationProvider _mapperConfig => _mapper.ConfigurationProvider;

    public UsersRepository(
        ShopManagerDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<TProjectTo?> GetByIdAsync<TProjectTo>(Guid id) where TProjectTo : class
    {
        var userEntity = await _context.Users
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

        if (typeof(TProjectTo) == typeof(UserEntity))
        {
            return userEntity as TProjectTo;
        }

        return _mapper.Map<TProjectTo>(userEntity);
    }

    public async Task<User[]> GetByIdsAsync(IEnumerable<Guid> userIds)
    {
        var usersEntity = await _context.Users
            .AsNoTracking()
            .Where(u => userIds.Contains(u.Id))
            .ToArrayAsync();

        var users = _mapper.Map<UserEntity[], User[]>(usersEntity);
        return users;
    }

    public async Task<TProjectTo?> GetByEmailAsync<TProjectTo>(string email) where TProjectTo : class
    {
        var userEntity = await _context.Users
            .Where(x => x.Email == email)
            .FirstOrDefaultAsync();

        if (typeof(TProjectTo) == typeof(UserEntity))
        {
            return userEntity as TProjectTo;
        }

        return _mapper.Map<TProjectTo>(userEntity);
    }

    public async Task<User[]> GetAsync()
    {
        var usersEntities = await _context.Users
            .AsNoTracking()
            .ToArrayAsync();

        var users = _mapper.Map<UserEntity[], User[]>(usersEntities);
        return users;
    }
    
    public async Task<Result<List<TProjectTo>>> GetBirthdayUsersAsync<TProjectTo>(int day, int month) => 
        await _context.Users
            .AsNoTracking()
            .Where(u => u.DateOfBirth.Month == month && u.DateOfBirth.Day == day)
            .ProjectTo<TProjectTo>(_mapperConfig)
            .ToListAsync();
    
    public async Task<bool> Delete(Guid id)
    {
        var userToDelete = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == id);

        if (userToDelete is null)
        {
            return false;
        }

        _context.Remove(userToDelete);
        await _context.SaveChangesAsync();

        return true;
    }
}
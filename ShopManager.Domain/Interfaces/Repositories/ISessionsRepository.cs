using CSharpFunctionalExtensions;
using ShopManager.Domain.Models;

namespace ShopManager.Domain.Interfaces.Repositories;

public interface ISessionsRepository
{
    Task<Result<Session>> GetById(Guid userId);

    Task<Result<bool>> Create(Session session);

    Task<Result<bool>> Delete(Guid userId);
}
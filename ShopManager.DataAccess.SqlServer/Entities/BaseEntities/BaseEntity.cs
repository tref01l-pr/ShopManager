using ShopManager.Domain.Interfaces.BaseInterfaces;
using ShopManager.Domain.Interfaces.Repositories;

namespace ShopManager.DataAccess.SqlServer.Entities.BaseEntities;

public abstract class BaseEntity<T> : IDbKey<T>
{
    public T Id { get; set; }
}
namespace ShopManager.Domain.Interfaces.BaseInterfaces;

public interface IModelKey<TKey>
{
    TKey Id { get; init; }
}
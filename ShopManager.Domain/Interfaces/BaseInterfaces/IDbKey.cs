namespace ShopManager.Domain.Interfaces.BaseInterfaces;

public interface IDbKey<TKey>
{
    TKey Id { get; set; }
}
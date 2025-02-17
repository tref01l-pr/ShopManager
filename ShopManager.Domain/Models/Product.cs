using CSharpFunctionalExtensions;
using ShopManager.Domain.Interfaces;
using ShopManager.Domain.Interfaces.BaseInterfaces;

namespace ShopManager.Domain.Models;

public record Product : IModelKey<int>
{
    public const int MaxNameLength = 200;
    public const int MaxArticleNumberLength = 50;
    public const decimal MinPrice = 0.01m;
    public const decimal MaxPrice = 999999.99m;

    public int Id { get; init; }
    public int CategoryId { get; }
    public string Name { get; }
    public string ArticleNumber { get; }
    public decimal Price { get; }

    private Product(int id, int categoryId, string name, string articleNumber, decimal price)
    {
        Id = id;
        CategoryId = categoryId;
        Name = name;
        ArticleNumber = articleNumber;
        Price = price;
    }

    public static Result<Product> Create(
        int categoryId,
        string name,
        string articleNumber,
        decimal price)
    {
        if (categoryId <= 0)
        {
            return Result.Failure<Product>($"Product {nameof(categoryId)} can't be less than or equal to 0");
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Product>($"Product {nameof(name)} can't be null or white space");
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<Product>($"Product {nameof(name)} can't be more than {MaxNameLength} chars");
        }

        if (string.IsNullOrWhiteSpace(articleNumber))
        {
            return Result.Failure<Product>($"Product {nameof(articleNumber)} can't be null or white space");
        }

        if (articleNumber.Length > MaxArticleNumberLength)
        {
            return Result.Failure<Product>(
                $"Product {nameof(articleNumber)} can't be more than {MaxArticleNumberLength} chars");
        }

        if (price is < MinPrice or > MaxPrice)
        {
            return Result.Failure<Product>($"Product {nameof(price)} must be between {MinPrice} and {MaxPrice}");
        }

        return new Product(
            0,
            categoryId,
            name,
            articleNumber,
            price);
    }
}
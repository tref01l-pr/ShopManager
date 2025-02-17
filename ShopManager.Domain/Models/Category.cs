using CSharpFunctionalExtensions;
using ShopManager.Domain.Interfaces;
using ShopManager.Domain.Interfaces.BaseInterfaces;

namespace ShopManager.Domain.Models;

public record Category : IModelKey<int>
{
    public static int MaxNameLength = 100;

    public int Id { get; init; }
    public string Name { get; }

    private Category(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public static Result<Category> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Category>("name cannot be empty");
        }

        if (name.Length > MaxNameLength)
        {
            return Result.Failure<Category>($"name cannot be longer than {MaxNameLength} characters");
        }

        return new Category(0, name);
    }
}
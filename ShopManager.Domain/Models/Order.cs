using CSharpFunctionalExtensions;
using ShopManager.Domain.Interfaces.BaseInterfaces;

namespace ShopManager.Domain.Models;

public record Order : IModelKey<int>
{
    public const int MaxNumberLength = 50;
    public const decimal MinTotalAmount = 0.01m;
    public const decimal MaxTotalAmount = 9999999.99m;

    public int Id { get; init; }
    public Guid UserId { get; }
    public string Number { get; }
    public DateTime OrderDate { get; }
    public decimal TotalAmount { get; }

    private Order(int id, Guid userId, string number, DateTime orderDate, decimal totalAmount)
    {
        Id = id;
        UserId = userId;
        Number = number;
        OrderDate = orderDate;
        TotalAmount = totalAmount;
    }

    public static Result<Order> Create(Guid userId, decimal totalAmount)
    {

        if (userId == Guid.Empty)
        {
            return Result.Failure<Order>($"Order {nameof(userId)} cannot be empty");
        }

        if (totalAmount is < MinTotalAmount or > MaxTotalAmount)
        {
            return Result.Failure<Order>($"Order {nameof(totalAmount)} must be between {MinTotalAmount} and {MaxTotalAmount}");
        }

        var orderNumber = GenerateOrderNumber();
        if (orderNumber.Length > MaxNumberLength)
        {
            return Result.Failure<Order>($"Generated order number exceeds maximum length of {MaxNumberLength} characters");
        }

        return Result.Success(new Order(
            0,
            userId,
            orderNumber,
            DateTime.Now,
            totalAmount));
    }

    private static string GenerateOrderNumber()
    {
        var timestamp = DateTime.Now;
        var random = new Random();
        var randomPart = random.Next(1000, 9999);

        return $"ORD-{timestamp:yyyyMMdd}-{timestamp:HHmmss}-{randomPart}";
    }
}
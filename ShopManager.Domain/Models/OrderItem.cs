using CSharpFunctionalExtensions;
using ShopManager.Domain.Interfaces.BaseInterfaces;

namespace ShopManager.Domain.Models;

public record OrderItem : IModelKey<int>
{
    public const int MinQuantity = 1;
    public const int MaxQuantity = 999;
    public const decimal MinUnitPrice = 0.01m;
    public const decimal MaxUnitPrice = 999999.99m;

    public int Id { get; init; }
    public int OrderId { get; }
    public int ProductId { get; }
    public int Quantity { get; }
    public decimal UnitPrice { get; }

    private OrderItem(int id, int orderId, int productId, int quantity, decimal unitPrice)
    {
        Id = id;
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public static OrderItemBuilder Builder => new OrderItemBuilder();

    public class OrderItemBuilder
    {
        private int _id;
        private int _orderId;
        private int _productId;
        private int _quantity;
        private decimal _unitPrice;

        public OrderItemBuilder SetOrderId(int orderId)
        {
            _orderId = orderId;
            return this;
        }

        public OrderItemBuilder SetProductId(int productId)
        {
            _productId = productId;
            return this;
        }

        public OrderItemBuilder SetQuantity(int quantity)
        {
            _quantity = quantity;
            return this;
        }

        public OrderItemBuilder SetUnitPrice(decimal unitPrice)
        {
            _unitPrice = unitPrice;
            return this;
        }

        public Result<OrderItem> Build()
        {
            var validationResult = ValidateOrderItemData(_productId, _quantity, _unitPrice);

            if (validationResult.IsFailure)
            {
                return Result.Failure<OrderItem>(validationResult.Error);
            }

            return new OrderItem(_id, _orderId, _productId, _quantity, _unitPrice);
        }

        private static Result ValidateOrderItemData(int productId, int quantity, decimal unitPrice)
        {
            if (productId <= 0)
            {
                return Result.Failure($"OrderItem {nameof(productId)} can't be less than or equal to 0");
            }

            if (quantity is < MinQuantity or > MaxQuantity)
            {
                return Result.Failure($"OrderItem {nameof(quantity)} must be between {MinQuantity} and {MaxQuantity}");
            }

            if (unitPrice is < MinUnitPrice or > MaxUnitPrice)
            {
                return Result.Failure(
                    $"OrderItem {nameof(unitPrice)} must be between {MinUnitPrice} and {MaxUnitPrice}");
            }

            return Result.Success();
        }
    }
}
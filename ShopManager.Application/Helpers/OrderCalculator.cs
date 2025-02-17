using ShopManager.Domain.Dtos.OrderItemDtos;
using ShopManager.Domain.Models;

namespace ShopManager.Application.Helpers;

public static class OrderCalculator
{
    public static decimal CalculateByOrderItems(List<OrderItem> orderItems) =>
        orderItems.Sum(orderItem => orderItem.UnitPrice * orderItem.Quantity);

    public static Dictionary<int, int> RemoveDuplicatesOrderItems(List<OrderItemDto> orderItems)
    {
        //first is id of product, second is quantity of them
        Dictionary<int, int> updatedOrderItems = new Dictionary<int, int>();

        foreach (var orderItem in orderItems)
        {
            if (updatedOrderItems.TryGetValue(orderItem.ProductId, out int quantity))
            {
                updatedOrderItems[orderItem.ProductId] = quantity + orderItem.Quantity;
                continue;
            }

            updatedOrderItems.Add(orderItem.ProductId, orderItem.Quantity);
        }

        return updatedOrderItems;
    }
}
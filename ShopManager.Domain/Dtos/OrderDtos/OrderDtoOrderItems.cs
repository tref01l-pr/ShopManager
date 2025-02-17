using ShopManager.Domain.Dtos.OrderItemDtos;

namespace ShopManager.Domain.Dtos.OrderDtos;

public class OrderDtoOrderItems : OrderDto
{
    public List<OrderItemDtoProduct> OrderItems { get; set; }
}
using ShopManager.Domain.Dtos.ProductDtos;

namespace ShopManager.Domain.Dtos.OrderItemDtos;

public class OrderItemDtoProduct : OrderItemDto
{
    public int OrderId { get; set; }
    public decimal UnitPrice { get; set; }
    public ProductDto Product { get; set; }
}
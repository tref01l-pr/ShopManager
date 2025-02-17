namespace ShopManager.API.Contracts.Responses;

public class OrderItemWithProductResponse
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public ProductResponse Product { get; set; }
}
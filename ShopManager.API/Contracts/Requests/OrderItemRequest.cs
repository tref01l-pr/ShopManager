namespace ShopManager.API.Contracts.Requests;

public class OrderItemRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
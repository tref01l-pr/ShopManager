namespace ShopManager.API.Contracts.Requests;

public class CreateOrderRequest
{
    public List<OrderItemRequest> OrderItems { get; set; }
}
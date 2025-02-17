namespace ShopManager.API.Contracts.Responses;

public class OrderResponse
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string Number { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    
    public List<OrderItemWithProductResponse> OrderItems { get; set; }
}
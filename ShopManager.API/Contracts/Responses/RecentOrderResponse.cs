namespace ShopManager.API.Contracts.Responses;

public class RecentOrderResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public DateTime LastOrderDate { get; set; }
}
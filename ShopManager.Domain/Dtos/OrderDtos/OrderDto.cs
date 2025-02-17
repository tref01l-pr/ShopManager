namespace ShopManager.Domain.Dtos.OrderDtos;

public class OrderDto
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string Number { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
}
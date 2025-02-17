namespace ShopManager.Domain.Dtos.UserDtos;

public class RecentUserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public DateTime LastOrderDate { get; set; }
}
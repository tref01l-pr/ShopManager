using ShopManager.Domain.Dtos.UserDtos;

namespace ShopManager.Domain.Dtos.OrderDtos;

public class OrderDtoUser : OrderDto
{
    public UserDto User { get; set; }
}
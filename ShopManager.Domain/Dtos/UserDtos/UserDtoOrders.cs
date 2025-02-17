using ShopManager.Domain.Dtos.OrderDtos;

namespace ShopManager.Domain.Dtos.UserDtos;

public class UserDtoOrders : UserDto
{
    public List<OrderDto> Orders { get; set; }
}
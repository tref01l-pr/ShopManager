namespace ShopManager.API.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopManager.API.Contracts.Requests;
using ShopManager.Domain.Dtos.OrderDtos;
using ShopManager.Domain.Dtos.OrderItemDtos;
using ShopManager.Domain.Interfaces;

[Authorize]
public class OrdersController : BaseController
{
    private readonly ILogger<ProductsController> _logger;
    private readonly IOrdersService _ordersService;
    private readonly IMapper _mapper;

    public OrdersController(
        ILogger<ProductsController> logger,
        IOrdersService ordersService,
        IMapper mapper)
    {
        _logger = logger;
        _ordersService = ordersService;
        _mapper = mapper;
    }
    
    [HttpPost("create-order")]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest createOrderRequest)
    {
        var orderItemsDto = _mapper.Map<List<OrderItemRequest>, List<OrderItemDto>>(createOrderRequest.OrderItems);

        if (UserId.IsFailure)
        {
            return BadRequest(UserId.Error);
        }
        
        var order = await _ordersService.CreateAsync<OrderDtoOrderItems>(orderItemsDto, UserId.Value);

        if (order.IsFailure)
        {
            return BadRequest(order.Error);
        }

        return Ok(order.Value);
    }
}
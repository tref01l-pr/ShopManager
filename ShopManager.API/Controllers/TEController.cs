namespace ShopManager.API.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopManager.API.Contracts.Responses;
using ShopManager.Domain.Dtos.CategoryDtos;
using ShopManager.Domain.Dtos.UserDtos;
using ShopManager.Domain.Interfaces;

[Authorize]
public class TEController : BaseController
{
    private readonly ILogger<ProductsController> _logger;
    private readonly IUsersService _usersService;
    private readonly IOrdersService _ordersService;
    private readonly IMapper _mapper;
    private readonly ICategoriesService _categoriesService;

    public TEController(
        ILogger<ProductsController> logger,
        IUsersService usersService,
        IOrdersService ordersService,
        ICategoriesService categoriesService,
        IMapper mapper)
    {
        _logger = logger;
        _usersService = usersService;
        _ordersService = ordersService;
        _categoriesService = categoriesService;
        _mapper = mapper;
    }

    [HttpGet("birthdays")]
    public async Task<ActionResult> GetBirthdayUsers([FromQuery] string dateStr)
    {
        if (!DateOnly.TryParse(dateStr, out var date))
        {
            _logger.LogError("Invalid date format. Use YYYY-MM-DD");
            return BadRequest("Invalid date format. Use YYYY-MM-DD");
        }

        var users = await _usersService.GetBirthdayUsersAsync<UserDto>(date);

        if (users.IsFailure)
        {
            _logger.LogError("{error}", users.Error);
            return BadRequest("Error while getting users");
        }

        var response = _mapper.Map<List<UserDto>, List<UserBirthdayResponse>>(users.Value);

        return Ok(response);
    }

    [HttpGet("recent-customers")]
    public async Task<ActionResult> GetRecentCustomers([FromQuery] int days)
    {
        var users = await _ordersService.GetRecentOrdersAsync(days);

        if (users.IsFailure)
        {
            _logger.LogError("{error}", users.Error);
            return BadRequest("Error while getting recent orders");
        }

        var response = _mapper.Map<List<RecentUserDto>, List<RecentOrderResponse>>(users.Value);

        return Ok(response);
    }

    [HttpGet("user-categories")]
    public async Task<ActionResult> GetUserCategories()
    {
        if (UserId.IsFailure)
        {
            _logger.LogError(UserId.Error);
            return BadRequest("Incorrect UserId. It cannot be empty");
        }

        var categories = await _categoriesService.GetUserCategoriesAsync(UserId.Value);

        if (categories.IsFailure)
        {
            _logger.LogError("{error}", categories.Error);
            return BadRequest("Error while getting user categories");
        }

        var response = _mapper.Map<List<UserCategoryDto>, List<UserCategoryResponse>>(categories.Value);

        return Ok(response);
    }
}
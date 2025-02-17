using CSharpFunctionalExtensions;
using ShopManager.Application.Helpers;
using ShopManager.Application.Services.BaseServices;
using ShopManager.DataAccess.SqlServer.Entities;
using ShopManager.Domain.Dtos.CategoryDtos;
using ShopManager.Domain.Dtos.OrderDtos;
using ShopManager.Domain.Dtos.OrderItemDtos;
using ShopManager.Domain.Dtos.ProductDtos;
using ShopManager.Domain.Dtos.UserDtos;
using ShopManager.Domain.Interfaces;
using ShopManager.Domain.Interfaces.Repositories;
using ShopManager.Domain.Models;

namespace ShopManager.Application.Services;

public class OrdersService : CrudService<IOrdersRepository, OrderEntity, Order, int>, IOrdersService
{
    private readonly IOrderItemsRepository _orderItemsRepository;
    private readonly IProductsRepository _productsRepository;

    public OrdersService(
        IOrdersRepository repository,
        IOrderItemsRepository orderItemsRepository,
        IProductsRepository productsRepository,
        ITransactionsRepository transactionsRepository)
        : base(repository, transactionsRepository)
    {
        _orderItemsRepository = orderItemsRepository;
        _productsRepository = productsRepository;
    }

    public override async Task<Result<TProjectTo>> CreateAsync<TProjectTo>(Order model)
    {
        return Result.Failure<TProjectTo>("Use another one method");
    }

    public async Task<Result<TProjectTo>> CreateAsync<TProjectTo>(List<OrderItemDto> createOrderRequest, Guid userId)
    {
        var transaction = await _transactionsRepository.BeginTransactionAsync();
        try
        {
            if (!createOrderRequest.Any())
            {
                throw new Exception("Cannot create order without OrderItem");
            }

            Dictionary<int, int> productAndQuantity = OrderCalculator.RemoveDuplicatesOrderItems(createOrderRequest);

            List<OrderItem.OrderItemBuilder> orderItemBuilders = new List<OrderItem.OrderItemBuilder>();
            List<OrderItem> orderItems = new List<OrderItem>();
            foreach (var orderItem in productAndQuantity)
            {
                var product = await _productsRepository.GetByIdAsync<ProductDto>(orderItem.Key);
                if (product == null)
                {
                    throw new Exception("Product with that id not found!");
                }

                var builder = OrderItem.Builder
                    .SetProductId(orderItem.Key)
                    .SetQuantity(orderItem.Value)
                    .SetUnitPrice(product.Price);
                orderItemBuilders.Add(builder);

                var orderItemModelResult = builder.Build();
                if (orderItemModelResult.IsFailure)
                {
                    throw new Exception(orderItemModelResult.Error);
                }

                orderItems.Add(orderItemModelResult.Value);
            }

            var order = Order.Create(userId, OrderCalculator.CalculateByOrderItems(orderItems));

            if (order.IsFailure)
            {
                throw new Exception(order.Error);
            }

            var orderCreationResult = await _repository.CreateAsync<OrderDto>(order.Value);

            if (orderCreationResult.IsFailure)
            {
                throw new Exception(orderCreationResult.Error);
            }

            foreach (var orderItemBuilder in orderItemBuilders)
            {
                var orderItemModelResult = orderItemBuilder
                    .SetOrderId(orderCreationResult.Value.Id)
                    .Build();

                if (orderItemModelResult.IsFailure)
                {
                    throw new Exception("Cannot create order item");
                }

                var orderItemCreationResult =
                    await _orderItemsRepository.CreateAsync<OrderItemDto>(orderItemModelResult.Value);
                if (orderItemCreationResult.IsFailure)
                {
                    throw new Exception(orderItemCreationResult.Error);
                }
            }

            await _transactionsRepository.CommitTransactionAsync(transaction);

            var result = await _repository.GetByIdAsync<TProjectTo>(orderCreationResult.Value.Id);
            if (result == null)
            {
                throw new Exception("Order was not created");
            }

            return result;
        }
        catch (Exception e)
        {
            await _transactionsRepository.RollbackTransactionAsync(transaction);
            return Result.Failure<TProjectTo>(e.Message);
        }
    }

    public async Task<Result<List<RecentUserDto>>> GetRecentOrdersAsync(int days)
    {
        try
        {
            if (days <= 0)
            {
                throw new Exception("Days cannot be less than or equal to 0");
            }

            var users = await _repository.GetRecentUsersAsync(days);

            return users;
        }
        catch (Exception e)
        {
            return Result.Failure<List<RecentUserDto>>(e.Message);
        }
    }

    
}
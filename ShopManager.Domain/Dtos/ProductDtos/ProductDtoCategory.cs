using ShopManager.Domain.Models;

namespace ShopManager.Domain.Dtos.ProductDtos;

public class ProductDtoCategory : ProductDto
{
    public Category Category { get; init; }
}
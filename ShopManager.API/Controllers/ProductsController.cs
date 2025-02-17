namespace ShopManager.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using ShopManager.Domain.Dtos.ProductDtos;
using ShopManager.Domain.Interfaces;

public class ProductsController : BaseController
{
    private readonly ILogger<ProductsController> _logger;
    private readonly IProductsService _productsService;

    public ProductsController(
        ILogger<ProductsController> logger,
        IProductsService productsService)
    {
        _logger = logger;
        _productsService = productsService;
    }
    
    [HttpGet("get-all-products")]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productsService.GetAllAsync<ProductDtoCategory>();

        if (products.IsFailure)
        {
            return BadRequest(products.Error);
        }

        return Ok(products.Value);
    }
    
    [HttpGet("get-product/{productId:int}")]
    public async Task<IActionResult> GetProduct(int productId)
    {
        var product = await _productsService.GetByIdAsync<ProductDtoCategory>(productId);

        if (product.IsFailure)
        {
            return BadRequest(product.Error);
        }

        return Ok(product.Value);
    }
}
namespace ShopManager.Domain.Dtos.ProductDtos;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public string ArticleNumber { get; set; }
    public decimal Price { get; set; }
}
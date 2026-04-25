namespace OnlineBazar.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public bool IsFeatured { get; set; }
    public int CategoryId { get; set; }
    public string? ImageUrl { get; set; }
    public int ViewCount { get; set; }
    public int TotalSold { get; set; }
}

using OnlineBazar.DTOs;

namespace OnlineBazar.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetFeaturedAsync();
    Task<IEnumerable<ProductDto>> GetRandomProductsAsync(int count);
    Task<IEnumerable<ProductDto>> GetTrendingProductsAsync(int count);
    Task<IEnumerable<ProductDto>> GetCatalogAsync(string? categorySlug, string? search, int page, int pageSize);
    Task<ProductDto?> GetByIdAsync(int id);
    Task IncrementViewCountAsync(int id);
    Task CreateAsync(ProductDto dto);
    Task UpdateAsync(ProductDto dto);
    Task DeleteAsync(int id);
}

using OnlineBazar.DTOs;
using OnlineBazar.Interfaces;
using OnlineBazar.Models;

namespace OnlineBazar.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _uow;

    public ProductService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<IEnumerable<ProductDto>> GetFeaturedAsync()
    {
        var products = await _uow.Products.GetAllAsync(p => p.IsFeatured && !p.IsDeleted);
        return products.Cast<Product>().Select(MapToDto);
    }

    public async Task<IEnumerable<ProductDto>> GetRandomProductsAsync(int count)
    {
        var products = (await _uow.Products.GetAllAsync(p => !p.IsDeleted))
            .Cast<Product>()
            .OrderBy(_ => Random.Shared.Next())
            .Take(count);

        return products.Select(MapToDto);
    }

    public async Task<IEnumerable<ProductDto>> GetCatalogAsync(string? categorySlug, string? search, int page, int pageSize)
    {
        var products = (await _uow.Products.GetAllAsync(p => !p.IsDeleted)).Cast<Product>();

        if (!string.IsNullOrWhiteSpace(search))
            products = products.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(categorySlug))
        {
            var category = (await _uow.Categories.GetAllAsync(c => c.Slug == categorySlug && !c.IsDeleted)).Cast<Category>().FirstOrDefault();
            if (category != null)
                products = products.Where(p => p.CategoryId == category.Id);
        }

        return products.Skip((page - 1) * pageSize).Take(pageSize).Select(MapToDto);
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _uow.Products.GetByIdAsync(id) as Product;
        return product is null || product.IsDeleted ? null : MapToDto(product);
    }

    public async Task CreateAsync(ProductDto dto)
    {
        var entity = new Product
        {
            Name = dto.Name,
            Slug = dto.Slug,
            Description = dto.Description,
            Price = dto.Price,
            StockQuantity = dto.StockQuantity,
            IsFeatured = dto.IsFeatured,
            CategoryId = dto.CategoryId,
            ImageUrl = dto.ImageUrl
        };
        await _uow.Products.AddAsync(entity);
        await _uow.SaveChangesAsync();
    }

    public async Task UpdateAsync(ProductDto dto)
    {
        var product = await _uow.Products.GetByIdAsync(dto.Id) as Product;
        if (product is null) throw new KeyNotFoundException("Product not found.");

        product.Name = dto.Name;
        product.Slug = dto.Slug;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.StockQuantity = dto.StockQuantity;
        product.IsFeatured = dto.IsFeatured;
        product.CategoryId = dto.CategoryId;
        product.ImageUrl = dto.ImageUrl;
        product.ModifiedDate = DateTime.UtcNow;

        _uow.Products.Update(product);
        await _uow.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _uow.Products.GetByIdAsync(id) as Product;
        if (product is null) return;

        product.IsDeleted = true;
        product.ModifiedDate = DateTime.UtcNow;
        _uow.Products.Update(product);
        await _uow.SaveChangesAsync();
    }

    private static ProductDto MapToDto(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Slug = product.Slug,
        Description = product.Description,
        Price = product.Price,
        StockQuantity = product.StockQuantity,
        IsFeatured = product.IsFeatured,
        CategoryId = product.CategoryId,
        ImageUrl = product.ImageUrl
    };
}

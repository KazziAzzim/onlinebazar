using Microsoft.EntityFrameworkCore;
using OnlineBazar.Data;
using OnlineBazar.DTOs;
using OnlineBazar.Interfaces;
using OnlineBazar.Models;

namespace OnlineBazar.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _uow;
    private readonly ApplicationDbContext _dbContext;

    public ProductService(IUnitOfWork uow, ApplicationDbContext dbContext)
    {
        _uow = uow;
        _dbContext = dbContext;
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

    public async Task<IEnumerable<ProductDto>> GetTrendingProductsAsync(int count)
    {
        var hasOrders = await _dbContext.OrderItems.AsNoTracking().AnyAsync();

        if (!hasOrders)
        {
            return await _dbContext.Products
                .AsNoTracking()
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.ViewCount)
                .ThenBy(p => p.Name)
                .Take(count)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Slug = p.Slug,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    IsFeatured = p.IsFeatured,
                    CategoryId = p.CategoryId,
                    ImageUrl = p.ImageUrl,
                    ViewCount = p.ViewCount,
                    TotalSold = 0
                })
                .ToListAsync();
        }

        var trending = await _dbContext.Products
            .AsNoTracking()
            .Where(p => !p.IsDeleted)
            .GroupJoin(
                _dbContext.OrderItems.AsNoTracking(),
                product => product.Id,
                orderItem => orderItem.ProductId,
                (product, orderItems) => new
                {
                    Product = product,
                    TotalSold = orderItems.Sum(item => (int?)item.Quantity) ?? 0
                })
            .OrderByDescending(x => x.TotalSold)
            .ThenByDescending(x => x.Product.ViewCount)
            .ThenBy(x => x.Product.Name)
            .Take(count)
            .Select(x => new ProductDto
            {
                Id = x.Product.Id,
                Name = x.Product.Name,
                Slug = x.Product.Slug,
                Description = x.Product.Description,
                Price = x.Product.Price,
                StockQuantity = x.Product.StockQuantity,
                IsFeatured = x.Product.IsFeatured,
                CategoryId = x.Product.CategoryId,
                ImageUrl = x.Product.ImageUrl,
                ViewCount = x.Product.ViewCount,
                TotalSold = x.TotalSold
            })
            .ToListAsync();

        return trending;
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

    public async Task IncrementViewCountAsync(int id)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        if (product is null)
        {
            return;
        }

        product.ViewCount += 1;
        product.ModifiedDate = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
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
            ImageUrl = dto.ImageUrl,
            ViewCount = dto.ViewCount
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
        product.ViewCount = dto.ViewCount;
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
        ImageUrl = product.ImageUrl,
        ViewCount = product.ViewCount,
        TotalSold = 0
    };
}

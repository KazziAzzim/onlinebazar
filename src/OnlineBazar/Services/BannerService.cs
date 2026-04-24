using Microsoft.EntityFrameworkCore;
using OnlineBazar.Data;
using OnlineBazar.DTOs;
using OnlineBazar.Interfaces;
using OnlineBazar.Models;

namespace OnlineBazar.Services;

public class BannerService : IBannerService
{
    private readonly ApplicationDbContext _dbContext;

    public BannerService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<BannerDto>> GetAllAsync()
    {
        return await _dbContext.Banners
            .AsNoTracking()
            .OrderBy(b => b.DisplayOrder)
            .ThenByDescending(b => b.CreatedAt)
            .Select(MapToDtoExpression())
            .ToListAsync();
    }

    public async Task<IEnumerable<BannerDto>> GetActiveBannersAsync()
    {
        return await _dbContext.Banners
            .AsNoTracking()
            .Where(b => b.IsActive)
            .OrderBy(b => b.DisplayOrder)
            .ThenBy(b => b.Id)
            .Select(MapToDtoExpression())
            .ToListAsync();
    }

    public async Task<BannerDto?> GetByIdAsync(int id)
    {
        return await _dbContext.Banners
            .AsNoTracking()
            .Where(b => b.Id == id)
            .Select(MapToDtoExpression())
            .FirstOrDefaultAsync();
    }

    public async Task CreateAsync(BannerDto dto)
    {
        var entity = new Banner
        {
            Title = dto.Title,
            Subtitle = dto.Subtitle,
            ImageUrl = dto.ImageUrl,
            RedirectUrl = dto.RedirectUrl,
            DisplayOrder = dto.DisplayOrder,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.Banners.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(BannerDto dto)
    {
        var banner = await _dbContext.Banners.FirstOrDefaultAsync(b => b.Id == dto.Id);
        if (banner is null)
        {
            throw new KeyNotFoundException("Banner not found.");
        }

        banner.Title = dto.Title;
        banner.Subtitle = dto.Subtitle;
        banner.ImageUrl = dto.ImageUrl;
        banner.RedirectUrl = dto.RedirectUrl;
        banner.DisplayOrder = dto.DisplayOrder;
        banner.IsActive = dto.IsActive;
        banner.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var banner = await _dbContext.Banners.FirstOrDefaultAsync(b => b.Id == id);
        if (banner is null) return;

        _dbContext.Banners.Remove(banner);
        await _dbContext.SaveChangesAsync();
    }

    public async Task ToggleActiveAsync(int id)
    {
        var banner = await _dbContext.Banners.FirstOrDefaultAsync(b => b.Id == id);
        if (banner is null) return;

        banner.IsActive = !banner.IsActive;
        banner.UpdatedAt = DateTime.UtcNow;
        await _dbContext.SaveChangesAsync();
    }

    private static System.Linq.Expressions.Expression<Func<Banner, BannerDto>> MapToDtoExpression() => banner => new BannerDto
    {
        Id = banner.Id,
        Title = banner.Title,
        Subtitle = banner.Subtitle,
        ImageUrl = banner.ImageUrl,
        RedirectUrl = banner.RedirectUrl,
        DisplayOrder = banner.DisplayOrder,
        IsActive = banner.IsActive
    };
}

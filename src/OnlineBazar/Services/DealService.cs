using Microsoft.EntityFrameworkCore;
using OnlineBazar.Data;
using OnlineBazar.DTOs;
using OnlineBazar.Interfaces;
using OnlineBazar.Models;

namespace OnlineBazar.Services;

public class DealService : IDealService
{
    private readonly ApplicationDbContext _dbContext;

    public DealService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<DealDto>> GetAllAsync()
    {
        return await _dbContext.Deals
            .AsNoTracking()
            .OrderBy(d => d.DisplayOrder)
            .ThenByDescending(d => d.CreatedAt)
            .Select(MapToDtoExpression())
            .ToListAsync();
    }

    public async Task<IEnumerable<DealDto>> GetActiveDeals()
    {
        var now = DateTime.UtcNow;

        return await _dbContext.Deals
            .AsNoTracking()
            .Where(d => d.IsActive && d.StartDate <= now && d.EndDate >= now)
            .OrderBy(d => d.DisplayOrder)
            .ThenBy(d => d.Id)
            .Select(MapToDtoExpression())
            .ToListAsync();
    }

    public async Task<DealDto?> GetByIdAsync(int id)
    {
        return await _dbContext.Deals
            .AsNoTracking()
            .Where(d => d.Id == id)
            .Select(MapToDtoExpression())
            .FirstOrDefaultAsync();
    }

    public async Task CreateAsync(DealDto dto)
    {
        var entity = new Deal
        {
            Title = dto.Title,
            Description = dto.Description,
            ImageUrl = dto.ImageUrl,
            DiscountType = dto.DiscountType,
            DiscountValue = dto.DiscountValue,
            StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc),
            EndDate = DateTime.SpecifyKind(dto.EndDate, DateTimeKind.Utc),
            RedirectUrl = dto.RedirectUrl,
            DisplayOrder = dto.DisplayOrder,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.Deals.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(DealDto dto)
    {
        var deal = await _dbContext.Deals.FirstOrDefaultAsync(d => d.Id == dto.Id);
        if (deal is null)
        {
            throw new KeyNotFoundException("Deal not found.");
        }

        deal.Title = dto.Title;
        deal.Description = dto.Description;
        deal.ImageUrl = dto.ImageUrl;
        deal.DiscountType = dto.DiscountType;
        deal.DiscountValue = dto.DiscountValue;
        deal.StartDate = DateTime.SpecifyKind(dto.StartDate, DateTimeKind.Utc);
        deal.EndDate = DateTime.SpecifyKind(dto.EndDate, DateTimeKind.Utc);
        deal.RedirectUrl = dto.RedirectUrl;
        deal.DisplayOrder = dto.DisplayOrder;
        deal.IsActive = dto.IsActive;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var deal = await _dbContext.Deals.FirstOrDefaultAsync(d => d.Id == id);
        if (deal is null) return;

        _dbContext.Deals.Remove(deal);
        await _dbContext.SaveChangesAsync();
    }

    private static System.Linq.Expressions.Expression<Func<Deal, DealDto>> MapToDtoExpression() => deal => new DealDto
    {
        Id = deal.Id,
        Title = deal.Title,
        Description = deal.Description,
        ImageUrl = deal.ImageUrl,
        DiscountType = deal.DiscountType,
        DiscountValue = deal.DiscountValue,
        StartDate = deal.StartDate,
        EndDate = deal.EndDate,
        RedirectUrl = deal.RedirectUrl,
        DisplayOrder = deal.DisplayOrder,
        IsActive = deal.IsActive
    };
}

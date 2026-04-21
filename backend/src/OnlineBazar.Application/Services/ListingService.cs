using Microsoft.EntityFrameworkCore;
using OnlineBazar.Application.DTOs.Common;
using OnlineBazar.Application.DTOs.Listing;
using OnlineBazar.Application.Interfaces;
using OnlineBazar.Domain.Entities;
using OnlineBazar.Domain.Enums;

namespace OnlineBazar.Application.Services;

public class ListingService(IUnitOfWork uow) : IListingService
{
    public async Task<PagedResult<ListingReadDto>> GetPublicAsync(int page, int pageSize, CancellationToken ct = default)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = uow.Listings.Query()
            .Where(x => !x.IsDeleted && x.Status == ListingStatus.Published)
            .OrderByDescending(x => x.PublishedDate);

        var total = await query.CountAsync(ct);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(x => new ListingReadDto
            {
                Id = x.Id,
                Title = x.Title,
                Slug = x.Slug,
                Description = x.Description,
                Price = x.Price,
                Location = x.Location,
                Status = x.Status.ToString()
            }).ToListAsync(ct);

        return new PagedResult<ListingReadDto> { Items = items, PageNumber = page, PageSize = pageSize, TotalCount = total };
    }

    public async Task<ListingReadDto> CreateAsync(ListingCreateDto dto, string actor, CancellationToken ct = default)
    {
        var listing = new Listing
        {
            Title = dto.Title.Trim(),
            Slug = dto.Title.Trim().ToLowerInvariant().Replace(" ", "-") + "-" + Guid.NewGuid().ToString("N")[..6],
            Description = dto.Description,
            CategoryId = dto.CategoryId,
            Price = dto.Price,
            Location = dto.Location,
            CreatedBy = actor,
            Status = ListingStatus.Draft
        };

        await uow.Listings.AddAsync(listing, ct);
        await uow.SaveChangesAsync(ct);

        return new ListingReadDto
        {
            Id = listing.Id,
            Title = listing.Title,
            Slug = listing.Slug,
            Description = listing.Description,
            Price = listing.Price,
            Location = listing.Location,
            Status = listing.Status.ToString()
        };
    }
}

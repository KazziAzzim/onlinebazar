using OnlineBazar.Application.DTOs.Common;
using OnlineBazar.Application.DTOs.Listing;

namespace OnlineBazar.Application.Interfaces;

public interface IListingService
{
    Task<PagedResult<ListingReadDto>> GetPublicAsync(int page, int pageSize, CancellationToken ct = default);
    Task<ListingReadDto> CreateAsync(ListingCreateDto dto, string actor, CancellationToken ct = default);
}

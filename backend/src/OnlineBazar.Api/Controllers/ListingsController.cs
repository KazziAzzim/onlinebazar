using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBazar.Application.DTOs.Listing;
using OnlineBazar.Application.Interfaces;
using System.Security.Claims;

namespace OnlineBazar.Api.Controllers;

[ApiController]
[Route("api/listings")]
public class ListingsController(IListingService listingService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetPublic([FromQuery] int page = 1, [FromQuery] int pageSize = 12, CancellationToken ct = default)
        => Ok(await listingService.GetPublicAsync(page, pageSize, ct));

    [HttpPost]
    [Authorize(Policy = "ManagerOrAdmin")]
    public async Task<IActionResult> Create([FromBody] ListingCreateDto request, CancellationToken ct)
    {
        var actor = User.FindFirstValue(ClaimTypes.Email) ?? "system";
        var created = await listingService.CreateAsync(request, actor, ct);
        return CreatedAtAction(nameof(GetPublic), new { id = created.Id }, created);
    }
}

using Microsoft.AspNetCore.Mvc;
using OnlineBazar.Interfaces;

namespace OnlineBazar.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BannersController : ControllerBase
{
    private readonly IBannerService _bannerService;

    public BannersController(IBannerService bannerService)
    {
        _bannerService = bannerService;
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActiveBanners()
    {
        var banners = await _bannerService.GetActiveBannersAsync();
        return Ok(banners);
    }
}

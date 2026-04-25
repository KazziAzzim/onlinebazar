using Microsoft.AspNetCore.Mvc;
using OnlineBazar.Interfaces;
using OnlineBazar.ViewModels;

namespace OnlineBazar.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly IBannerService _bannerService;
    private readonly IDealService _dealService;

    public HomeController(IProductService productService, IBannerService bannerService, IDealService dealService)
    {
        _productService = productService;
        _bannerService = bannerService;
        _dealService = dealService;
    }

    public async Task<IActionResult> Index()
    {
        var model = new HomeViewModel
        {
            FeaturedProducts = await _productService.GetFeaturedAsync(),
            TrendingProducts = await _productService.GetTrendingProductsAsync(10),
            Banners = await _bannerService.GetActiveBannersAsync(),
            Deals = await _dealService.GetActiveDeals()
        };

        return View(model);
    }

    [HttpGet("/api/products/trending")]
    public async Task<IActionResult> GetTrendingProducts(int count = 10)
    {
        var normalizedCount = Math.Clamp(count, 1, 20);
        var products = await _productService.GetTrendingProductsAsync(normalizedCount);
        return Ok(products);
    }

    public IActionResult About() => View();
    public IActionResult Contact() => View();
}

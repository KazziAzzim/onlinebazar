using Microsoft.AspNetCore.Mvc;
using OnlineBazar.Interfaces;
using OnlineBazar.ViewModels;

namespace OnlineBazar.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;

    public HomeController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        var model = new HomeViewModel
        {
            FeaturedProducts = await _productService.GetRandomProductsAsync(10)
        };

        return View(model);
    }

    public IActionResult About() => View();
    public IActionResult Contact() => View();
}

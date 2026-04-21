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
        var vm = new HomeViewModel
        {
            FeaturedProducts = await _productService.GetFeaturedAsync()
        };
        return View(vm);
    }

    public IActionResult About() => View();
    public IActionResult Contact() => View();
}

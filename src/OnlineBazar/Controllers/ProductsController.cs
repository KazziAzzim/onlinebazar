using Microsoft.AspNetCore.Mvc;
using OnlineBazar.Interfaces;

namespace OnlineBazar.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index(string? category, string? search, int page = 1)
    {
        var products = await _productService.GetCatalogAsync(category, search, page, 12);
        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        await _productService.IncrementViewCountAsync(id);
        var product = await _productService.GetByIdAsync(id);
        if (product is null) return NotFound();
        return View(product);
    }
}

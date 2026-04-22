using Microsoft.AspNetCore.Mvc;
using OnlineBazar.Interfaces;
using OnlineBazar.ViewModels;

namespace OnlineBazar.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;

    public HomeController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        var featuredProducts = (await _productService.GetFeaturedAsync()).ToList();
        var categories = (await _categoryService.GetAllAsync())
            .OrderBy(c => c.Name)
            .ToList();

        var categoryGroups = new List<CategoryProductGroupViewModel>();

        foreach (var category in categories)
        {
            var products = (await _productService.GetCatalogAsync(category.Slug, null, 1, 8)).ToList();
            if (!products.Any())
            {
                continue;
            }

            categoryGroups.Add(new CategoryProductGroupViewModel
            {
                CategoryName = category.Name,
                CategorySlug = category.Slug,
                Products = products
            });
        }

        var carouselProducts = categoryGroups
            .SelectMany(g => g.Products)
            .Take(6)
            .ToList();

        if (!carouselProducts.Any())
        {
            carouselProducts = featuredProducts.Take(6).ToList();
        }

        var vm = new HomeViewModel
        {
            FeaturedProducts = featuredProducts,
            CategoryGroups = categoryGroups,
            CarouselProducts = carouselProducts
        };

        return View(vm);
    }

    public IActionResult About() => View();
    public IActionResult Contact() => View();
}

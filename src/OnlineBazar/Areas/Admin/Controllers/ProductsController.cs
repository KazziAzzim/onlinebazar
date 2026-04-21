using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBazar.DTOs;
using OnlineBazar.Interfaces;

namespace OnlineBazar.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Manager")]
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly IWebHostEnvironment _env;

    public ProductsController(IProductService productService, IWebHostEnvironment env)
    {
        _productService = productService;
        _env = env;
    }

    public async Task<IActionResult> Index() => View(await _productService.GetCatalogAsync(null, null, 1, 100));

    public IActionResult Create() => View(new ProductDto());

    [HttpPost]
    public async Task<IActionResult> Create(ProductDto dto, IFormFile? image)
    {
        if (!ModelState.IsValid) return View(dto);

        if (image != null)
        {
            var fileName = $"{Guid.NewGuid()}_{image.FileName}";
            var path = Path.Combine(_env.WebRootPath, "uploads", fileName);
            using var stream = System.IO.File.Create(path);
            await image.CopyToAsync(stream);
            dto.ImageUrl = $"/uploads/{fileName}";
        }

        await _productService.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        return product is null ? NotFound() : View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        await _productService.UpdateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}

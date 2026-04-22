using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineBazar.DTOs;
using OnlineBazar.Interfaces;

namespace OnlineBazar.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Manager")]
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly ICategoryService _categoryService;
    private readonly IWebHostEnvironment _env;

    public ProductsController(IProductService productService, ICategoryService categoryService, IWebHostEnvironment env)
    {
        _productService = productService;
        _categoryService = categoryService;
        _env = env;
    }

    public async Task<IActionResult> Index() => View(await _productService.GetCatalogAsync(null, null, 1, 100));

    public async Task<IActionResult> Create()
    {
        await LoadCategoryOptionsAsync();
        return View(new ProductDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductDto dto, IFormFile? image)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoryOptionsAsync(dto.CategoryId);
            return View(dto);
        }

        if (image != null)
        {
            var webRoot = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var uploadsDir = Path.Combine(webRoot, "uploads");

            if (!Directory.Exists(uploadsDir))
            {
                Directory.CreateDirectory(uploadsDir);
            }

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(image.FileName)}";
            var path = Path.Combine(uploadsDir, fileName);

            try
            {
                await using var stream = System.IO.File.Create(path);
                await image.CopyToAsync(stream);
                dto.ImageUrl = $"/uploads/{fileName}";
            }
            catch
            {
                ModelState.AddModelError(string.Empty, "Unable to save the image. Please try again.");
                await LoadCategoryOptionsAsync(dto.CategoryId);
                return View(dto);
            }
        }

        await _productService.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product is null) return NotFound();

        await LoadCategoryOptionsAsync(product.CategoryId);
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductDto dto)
    {
        if (!ModelState.IsValid)
        {
            await LoadCategoryOptionsAsync(dto.CategoryId);
            return View(dto);
        }

        await _productService.UpdateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadCategoryOptionsAsync(int? selectedCategoryId = null)
    {
        var categories = await _categoryService.GetAllAsync();
        ViewBag.Categories = categories
            .Select(c => new SelectListItem(c.Name, c.Id.ToString(), c.Id == selectedCategoryId))
            .ToList();
    }
}

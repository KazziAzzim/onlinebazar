using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBazar.DTOs;
using OnlineBazar.Interfaces;

namespace OnlineBazar.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Manager")]
public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index() => View(await _categoryService.GetAllAsync());

    public IActionResult Create() => View(new CategoryDto());

    [HttpPost]
    public async Task<IActionResult> Create(CategoryDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        await _categoryService.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var category = await _categoryService.GetByIdAsync(id);
        return category is null ? NotFound() : View(category);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CategoryDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        await _categoryService.UpdateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _categoryService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}

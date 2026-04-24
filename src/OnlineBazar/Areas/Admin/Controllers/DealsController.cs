using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBazar.DTOs;
using OnlineBazar.Interfaces;

namespace OnlineBazar.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Manager")]
public class DealsController : Controller
{
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp", ".svg"
    };

    private readonly IDealService _dealService;
    private readonly IWebHostEnvironment _environment;

    public DealsController(IDealService dealService, IWebHostEnvironment environment)
    {
        _dealService = dealService;
        _environment = environment;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _dealService.GetAllAsync());
    }

    public IActionResult Create() => View(new DealDto { IsActive = true, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(1) });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DealDto dto, IFormFile? image)
    {
        ValidateDates(dto);
        ValidateRedirectUrl(dto);

        if (image is not null)
        {
            var imagePath = await SaveImageAsync(image);
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                return View(dto);
            }

            dto.ImageUrl = imagePath;
        }

        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        await _dealService.CreateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var deal = await _dealService.GetByIdAsync(id);
        return deal is null ? NotFound() : View(deal);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(DealDto dto, IFormFile? image)
    {
        ValidateDates(dto);
        ValidateRedirectUrl(dto);

        if (image is not null)
        {
            var imagePath = await SaveImageAsync(image);
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                return View(dto);
            }

            dto.ImageUrl = imagePath;
        }

        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        await _dealService.UpdateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _dealService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private void ValidateDates(DealDto dto)
    {
        if (dto.EndDate <= dto.StartDate)
        {
            ModelState.AddModelError(nameof(dto.EndDate), "End date must be later than start date.");
        }
    }

    private void ValidateRedirectUrl(DealDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.RedirectUrl))
        {
            dto.RedirectUrl = null;
            return;
        }

        var isAbsolute = Uri.TryCreate(dto.RedirectUrl, UriKind.Absolute, out var absoluteUri)
            && (absoluteUri.Scheme == Uri.UriSchemeHttp || absoluteUri.Scheme == Uri.UriSchemeHttps);
        var isRootRelative = dto.RedirectUrl.StartsWith('/');

        if (!isAbsolute && !isRootRelative)
        {
            ModelState.AddModelError(nameof(dto.RedirectUrl), "Redirect URL must be an absolute URL or a relative path starting with '/'.");
        }
    }

    private async Task<string?> SaveImageAsync(IFormFile image)
    {
        var extension = Path.GetExtension(image.FileName);

        if (string.IsNullOrWhiteSpace(extension) || !AllowedExtensions.Contains(extension))
        {
            ModelState.AddModelError("image", "Only .jpg, .jpeg, .png, .webp, and .svg files are allowed.");
            return null;
        }

        var webRoot = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var uploadFolder = Path.Combine(webRoot, "uploads", "deals");
        Directory.CreateDirectory(uploadFolder);

        var fileName = $"{Guid.NewGuid():N}{extension}";
        var fullPath = Path.Combine(uploadFolder, fileName);

        try
        {
            await using var stream = System.IO.File.Create(fullPath);
            await image.CopyToAsync(stream);
            return $"/uploads/deals/{fileName}";
        }
        catch
        {
            ModelState.AddModelError("image", "Unable to save image. Please try again.");
            return null;
        }
    }
}

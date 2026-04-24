using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBazar.DTOs;
using OnlineBazar.Interfaces;

namespace OnlineBazar.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Manager")]
public class BannersController : Controller
{
    private static readonly HashSet<string> AllowedExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".webp", ".svg"
    };

    private readonly IBannerService _bannerService;
    private readonly IWebHostEnvironment _environment;

    public BannersController(IBannerService bannerService, IWebHostEnvironment environment)
    {
        _bannerService = bannerService;
        _environment = environment;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _bannerService.GetAllAsync());
    }

    public IActionResult Create() => View(new BannerDto { IsActive = true });

    [HttpPost]
    public async Task<IActionResult> Create(BannerDto dto, IFormFile? image)
    {
        ValidateRedirectUrl(dto);

        if (image is null)
        {
            ModelState.AddModelError("image", "Banner image is required.");
        }



        var imagePath = await SaveImageAsync(image!);
        if (string.IsNullOrWhiteSpace(imagePath))
        {
            return View(dto);
        }

        dto.ImageUrl = imagePath;
        if (!ModelState.IsValid)
        {
            return View(dto);
        }
        await _bannerService.CreateAsync(dto);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var banner = await _bannerService.GetByIdAsync(id);
        return banner is null ? NotFound() : View(banner);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(BannerDto dto, IFormFile? image)
    {
        ValidateRedirectUrl(dto);

        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        if (image is not null)
        {
            var imagePath = await SaveImageAsync(image);
            if (string.IsNullOrWhiteSpace(imagePath))
            {
                return View(dto);
            }

            dto.ImageUrl = imagePath;
        }

        await _bannerService.UpdateAsync(dto);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _bannerService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> ToggleActive(int id)
    {
        await _bannerService.ToggleActiveAsync(id);
        return RedirectToAction(nameof(Index));
    }

    private void ValidateRedirectUrl(BannerDto dto)
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
            ModelState.AddModelError("image", "Only .jpg, .jpeg, .png, and .webp files are allowed.");
            return null;
        }

        var webRoot = _environment.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var uploadFolder = Path.Combine(webRoot, "uploads", "banners");
        Directory.CreateDirectory(uploadFolder);

        var fileName = $"{Guid.NewGuid():N}{extension}";
        var fullPath = Path.Combine(uploadFolder, fileName);

        try
        {
            await using var stream = System.IO.File.Create(fullPath);
            await image.CopyToAsync(stream);
            return $"/uploads/banners/{fileName}";
        }
        catch
        {
            ModelState.AddModelError("image", "Unable to save image. Please try again.");
            return null;
        }
    }
}

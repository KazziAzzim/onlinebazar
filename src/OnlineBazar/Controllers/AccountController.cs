using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using OnlineBazar.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineBazar.Models;
using OnlineBazar.ViewModels;

namespace OnlineBazar.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _dbContext;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext dbContext)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password, string returnUrl)
    {
        var result = await _signInManager.PasswordSignInAsync(email, password, true, false);
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Invalid credentials.");
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        var user = await _userManager.FindByEmailAsync(email);
        if (user is not null)
        {
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            var isManager = await _userManager.IsInRoleAsync(user, "Manager");
            if (isAdmin || isManager)
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var existing = await _userManager.FindByEmailAsync(model.Email);
        if (existing is not null)
        {
            ModelState.AddModelError(nameof(model.Email), "Email is already registered.");
            return View(model);
        }

        var user = new ApplicationUser
        {
            Email = model.Email,
            UserName = model.Email,
            FullName = model.FullName,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        await _userManager.AddToRoleAsync(user, "Customer");
        await _signInManager.SignInAsync(user, isPersistent: true);

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return RedirectToAction(nameof(Login));
        }

        var model = new UserProfileViewModel
        {
            FullName = user.FullName ?? string.Empty,
            Email = user.Email ?? string.Empty
        };

        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(UserProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return RedirectToAction(nameof(Login));
        }

        user.FullName = model.FullName;
        var updateResult = await _userManager.UpdateAsync(user);
        if (!updateResult.Succeeded)
        {
            foreach (var error in updateResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        TempData["ProfileMessage"] = "Profile updated successfully.";
        return RedirectToAction(nameof(Profile));
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> MyOrders()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return RedirectToAction(nameof(Login));
        }

        var orders = await _dbContext.Orders
            .AsNoTracking()
            .Where(o => o.UserId == user.Id && !o.IsDeleted)
            .OrderByDescending(o => o.CreatedDate)
            .Select(o => new OrderSummaryViewModel
            {
                Id = o.Id,
                CreatedDate = o.CreatedDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount
            })
            .ToListAsync();

        return View(orders);
    }

    [Authorize]
    [HttpGet]
    public IActionResult ChangePassword() => View(new ChangePasswordViewModel());

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return RedirectToAction(nameof(Login));
        }

        var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        await _signInManager.RefreshSignInAsync(user);
        TempData["PasswordMessage"] = "Password changed successfully.";
        return RedirectToAction(nameof(ChangePassword));
    }
}

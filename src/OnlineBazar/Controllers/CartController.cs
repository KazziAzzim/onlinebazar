using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBazar.Data;
using OnlineBazar.Models;

namespace OnlineBazar.Controllers;

[Authorize]
public class CartController : Controller
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public CartController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Challenge();
        }

        var cartOrder = await _dbContext.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.UserId == userId && o.Status == "Cart" && !o.IsDeleted);

        return View(cartOrder);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int productId, int quantity = 1)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Challenge();
        }

        if (quantity < 1)
        {
            quantity = 1;
        }

        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId && !p.IsDeleted);
        if (product is null)
        {
            return NotFound();
        }

        var cartOrder = await _dbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.UserId == userId && o.Status == "Cart" && !o.IsDeleted);

        if (cartOrder is null)
        {
            cartOrder = new Order
            {
                UserId = userId,
                Status = "Cart",
                ShippingAddress = string.Empty,
                TotalAmount = 0m,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };
            await _dbContext.Orders.AddAsync(cartOrder);
        }

        var existingItem = cartOrder.Items.FirstOrDefault(i => i.ProductId == productId && !i.IsDeleted);
        if (existingItem is null)
        {
            cartOrder.Items.Add(new OrderItem
            {
                ProductId = productId,
                Quantity = quantity,
                UnitPrice = product.Price,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            });
        }
        else
        {
            existingItem.Quantity += quantity;
            existingItem.ModifiedDate = DateTime.UtcNow;
        }

        cartOrder.TotalAmount = cartOrder.Items.Where(i => !i.IsDeleted).Sum(i => i.Quantity * i.UnitPrice);
        cartOrder.ModifiedDate = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        TempData["SuccessMessage"] = "Item added to cart.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int itemId)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Challenge();
        }

        var cartOrder = await _dbContext.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.UserId == userId && o.Status == "Cart" && !o.IsDeleted);
        if (cartOrder is null)
        {
            return RedirectToAction(nameof(Index));
        }

        var item = cartOrder.Items.FirstOrDefault(i => i.Id == itemId && !i.IsDeleted);
        if (item is not null)
        {
            item.IsDeleted = true;
            item.ModifiedDate = DateTime.UtcNow;

            cartOrder.TotalAmount = cartOrder.Items.Where(i => !i.IsDeleted).Sum(i => i.Quantity * i.UnitPrice);
            cartOrder.ModifiedDate = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}

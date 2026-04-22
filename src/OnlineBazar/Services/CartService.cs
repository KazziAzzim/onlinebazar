using Microsoft.EntityFrameworkCore;
using OnlineBazar.Data;
using OnlineBazar.Extensions;
using OnlineBazar.Interfaces;
using OnlineBazar.ViewModels;

namespace OnlineBazar.Services;

public class CartService : ICartService
{
    private const string CartSessionKey = "ONLINEBAZAR_CART";

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _dbContext;

    public CartService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    public async Task<CartViewModel> GetCartAsync()
    {
        var items = GetItemsFromSession();
        if (!items.Any())
        {
            return new CartViewModel();
        }

        var productIds = items.Select(i => i.ProductId).ToList();
        var products = await _dbContext.Products
            .Where(p => productIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        foreach (var item in items.ToList())
        {
            if (!products.TryGetValue(item.ProductId, out var product))
            {
                items.Remove(item);
                continue;
            }

            item.ProductName = product.Name;
            item.UnitPrice = product.Price;
            item.ImageUrl = product.ImageUrl;
            item.AvailableStock = product.StockQuantity;
            if (item.Quantity > product.StockQuantity)
            {
                item.Quantity = Math.Max(1, product.StockQuantity);
            }
        }

        SaveItemsToSession(items);
        return new CartViewModel { Items = items };
    }

    public async Task AddItemAsync(int productId, int quantity = 1)
    {
        quantity = Math.Max(1, quantity);
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
        if (product is null || product.StockQuantity <= 0)
        {
            return;
        }

        var items = GetItemsFromSession();
        var existing = items.FirstOrDefault(i => i.ProductId == productId);
        if (existing is null)
        {
            items.Add(new CartItemViewModel
            {
                ProductId = product.Id,
                ProductName = product.Name,
                UnitPrice = product.Price,
                Quantity = Math.Min(quantity, product.StockQuantity),
                ImageUrl = product.ImageUrl,
                AvailableStock = product.StockQuantity
            });
        }
        else
        {
            existing.Quantity = Math.Min(existing.Quantity + quantity, product.StockQuantity);
            existing.UnitPrice = product.Price;
            existing.ProductName = product.Name;
            existing.ImageUrl = product.ImageUrl;
            existing.AvailableStock = product.StockQuantity;
        }

        SaveItemsToSession(items);
    }

    public async Task UpdateQuantityAsync(int productId, int quantity)
    {
        var items = GetItemsFromSession();
        var item = items.FirstOrDefault(i => i.ProductId == productId);
        if (item is null)
        {
            return;
        }

        if (quantity <= 0)
        {
            items.Remove(item);
        }
        else
        {
            var stock = await _dbContext.Products
                .Where(p => p.Id == productId)
                .Select(p => p.StockQuantity)
                .FirstOrDefaultAsync();

            if (stock <= 0)
            {
                items.Remove(item);
            }
            else
            {
                item.Quantity = Math.Min(quantity, stock);
            }
        }

        SaveItemsToSession(items);
    }

    public Task RemoveItemAsync(int productId)
    {
        var items = GetItemsFromSession();
        var item = items.FirstOrDefault(i => i.ProductId == productId);
        if (item is not null)
        {
            items.Remove(item);
            SaveItemsToSession(items);
        }

        return Task.CompletedTask;
    }

    public int GetCartItemCount()
    {
        return GetItemsFromSession().Sum(i => i.Quantity);
    }

    private List<CartItemViewModel> GetItemsFromSession()
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        return session?.GetJson<List<CartItemViewModel>>(CartSessionKey) ?? [];
    }

    private void SaveItemsToSession(List<CartItemViewModel> items)
    {
        var session = _httpContextAccessor.HttpContext?.Session;
        session?.SetJson(CartSessionKey, items);
    }
}

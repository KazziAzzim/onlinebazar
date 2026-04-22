using OnlineBazar.ViewModels;

namespace OnlineBazar.Interfaces;

public interface ICartService
{
    Task<CartViewModel> GetCartAsync();
    Task AddItemAsync(int productId, int quantity = 1);
    Task UpdateQuantityAsync(int productId, int quantity);
    Task RemoveItemAsync(int productId);
    int GetCartItemCount();
}

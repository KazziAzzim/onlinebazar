namespace OnlineBazar.ViewModels;

public class CartViewModel
{
    public IList<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
    public int TotalItems => Items.Sum(i => i.Quantity);
    public decimal Subtotal => Items.Sum(i => i.LineTotal);
}

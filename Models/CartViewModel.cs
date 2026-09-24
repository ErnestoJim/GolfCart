namespace GolfCart.Models;

public class CartViewModel
{
    public IReadOnlyList<CartItem> Items { get; init; } = [];
    public decimal Total => Items.Sum(item => item.Total);
}

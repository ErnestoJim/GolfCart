using GolfCart.Models;

namespace GolfCart.Services;

public interface ICartService
{
    IReadOnlyList<CartItem> GetItems();
    int Count { get; }
    void Add(GolfBallProduct product);
    void UpdateQuantity(int productId, int quantity);
    void Remove(int productId);
    void Clear();
}

using System.Text.Json;
using GolfCart.Models;

namespace GolfCart.Services;

public class SessionCartService(IHttpContextAccessor httpContextAccessor) : ICartService
{
    private const string CartKey = "shopping-cart";
    private ISession Session => httpContextAccessor.HttpContext!.Session;

    public int Count => GetItems().Sum(item => item.Quantity);

    public IReadOnlyList<CartItem> GetItems()
    {
        var json = Session.GetString(CartKey);
        return string.IsNullOrWhiteSpace(json)
            ? []
            : JsonSerializer.Deserialize<List<CartItem>>(json) ?? [];
    }

    public void Add(GolfBallProduct product)
    {
        var items = GetItems().ToList();
        var item = items.SingleOrDefault(item => item.ProductId == product.Id);
        if (item is null)
        {
            items.Add(new CartItem
            {
                ProductId = product.Id,
                Description = $"{product.Brand} · {product.Grade.GetDisplayName()} · {product.PackSize} bolas",
                UnitPrice = product.Price,
                Quantity = 1
            });
        }
        else
        {
            item.Quantity++;
        }

        Save(items);
    }

    public void UpdateQuantity(int productId, int quantity)
    {
        var items = GetItems().ToList();
        var item = items.SingleOrDefault(item => item.ProductId == productId);
        if (item is null) return;

        if (quantity <= 0) items.Remove(item);
        else item.Quantity = quantity;
        Save(items);
    }

    public void Remove(int productId) => UpdateQuantity(productId, 0);
    public void Clear() => Session.Remove(CartKey);

    private void Save(List<CartItem> items) => Session.SetString(CartKey, JsonSerializer.Serialize(items));
}

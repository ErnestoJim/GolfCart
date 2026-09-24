using GolfCart.Data;
using GolfCart.Models;
using GolfCart.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GolfCart.Controllers;

public class CartController(GolfCartDbContext database, ICartService cart) : Controller
{
    public IActionResult Index() => View(new CartViewModel { Items = cart.GetItems() });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int productId, string? returnUrl)
    {
        var product = await database.Products.FindAsync(productId);
        if (product is null || !product.IsActive || product.StockPacks < 1)
        {
            TempData["CartError"] = "Este producto ya no está disponible.";
        }
        else if (cart.GetItems().SingleOrDefault(item => item.ProductId == productId)?.Quantity >= product.StockPacks)
        {
            TempData["CartError"] = "No puedes añadir más unidades que el stock disponible.";
        }
        else
        {
            cart.Add(product);
            TempData["CartMessage"] = "Producto añadido al carrito.";
        }

        return !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? LocalRedirect(returnUrl)
            : RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int productId, int quantity)
    {
        var product = await database.Products.FindAsync(productId);
        if (product is null || !product.IsActive || quantity > product.StockPacks)
        {
            TempData["CartError"] = "La cantidad supera el stock disponible o el producto ya no se vende.";
        }
        else
        {
            cart.UpdateQuantity(productId, quantity);
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove(int productId)
    {
        cart.Remove(productId);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Checkout()
    {
        var viewModel = new CartViewModel { Items = cart.GetItems() };
        if (viewModel.Items.Count == 0) return RedirectToAction(nameof(Index));
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmCheckout()
    {
        var items = cart.GetItems();
        if (items.Count == 0) return RedirectToAction(nameof(Index));

        await using var transaction = await database.Database.BeginTransactionAsync();
        var productIds = items.Select(item => item.ProductId).ToList();
        var products = await database.Products.Where(product => productIds.Contains(product.Id))
            .ToDictionaryAsync(product => product.Id);

        if (items.Any(item => !products.TryGetValue(item.ProductId, out var product) || !product.IsActive || product.StockPacks < item.Quantity))
        {
            await transaction.RollbackAsync();
            TempData["CartError"] = "El stock ha cambiado. Revisa tu carrito antes de confirmar.";
            return RedirectToAction(nameof(Index));
        }

        foreach (var item in items) products[item.ProductId].StockPacks -= item.Quantity;

        var order = new Order
        {
            Total = items.Sum(item => item.Total),
            Lines = items.Select(item => new OrderLine
            {
                ProductId = item.ProductId,
                Description = item.Description,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                LineTotal = item.Total
            }).ToList()
        };
        database.Orders.Add(order);
        await database.SaveChangesAsync();
        await transaction.CommitAsync();
        cart.Clear();

        return RedirectToAction(nameof(Confirmation), new { id = order.Id });
    }

    public async Task<IActionResult> Confirmation(int id)
    {
        var order = await database.Orders.Include(order => order.Lines).SingleOrDefaultAsync(order => order.Id == id);
        return order is null ? NotFound() : View(order);
    }
}

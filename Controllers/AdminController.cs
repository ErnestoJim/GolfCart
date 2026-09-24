using GolfCart.Data;
using GolfCart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GolfCart.Controllers;

[Authorize(Roles = ApplicationRoles.Admin)]
public class AdminController(GolfCartDbContext database) : Controller
{
    public async Task<IActionResult> Index() => View(await database.Products.OrderBy(product => product.Brand).ThenBy(product => product.Grade).ToListAsync());

    public IActionResult Create() => View(new GolfBallProduct());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(GolfBallProduct product)
    {
        if (!ModelState.IsValid) return View(product);
        product.Brand = product.Brand.Trim();
        database.Products.Add(product);
        await database.SaveChangesAsync();
        TempData["AdminMessage"] = "Producto creado.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await database.Products.FindAsync(id);
        return product is null ? NotFound() : View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, GolfBallProduct input)
    {
        if (id != input.Id) return NotFound();
        if (!ModelState.IsValid) return View(input);

        var product = await database.Products.FindAsync(id);
        if (product is null) return NotFound();

        product.Brand = input.Brand.Trim();
        product.Grade = input.Grade;
        product.PackSize = input.PackSize;
        product.Price = input.Price;
        product.StockPacks = input.StockPacks;
        product.IsActive = input.IsActive;
        await database.SaveChangesAsync();
        TempData["AdminMessage"] = "Inventario actualizado.";
        return RedirectToAction(nameof(Index));
    }
}

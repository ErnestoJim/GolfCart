using Microsoft.AspNetCore.Mvc;
using GolfCart.Models;
using GolfCart.Data;
using Microsoft.EntityFrameworkCore;

namespace GolfCart.Controllers;

public class HomeController(GolfCartDbContext database) : Controller
{
    public async Task<IActionResult> Index(string? brand, GolfBallGrade? grade)
    {
        var products = database.Products
            .Where(product => product.IsActive && product.StockPacks > 0)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(brand))
            products = products.Where(product => product.Brand == brand);
        if (grade.HasValue)
            products = products.Where(product => product.Grade == grade.Value);

        return View(new CatalogViewModel
        {
            Products = await products.OrderBy(product => product.Brand).ThenBy(product => product.Grade).ToListAsync(),
            Brand = brand,
            Grade = grade,
            Brands = await database.Products.Where(product => product.IsActive && product.StockPacks > 0)
                .Select(product => product.Brand).Distinct().OrderBy(brandName => brandName).ToListAsync()
        });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
    }
}

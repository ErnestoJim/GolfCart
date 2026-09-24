using GolfCart.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GolfCart.Data;

public static class GolfCartDbInitializer
{
    public static async Task InitializeAsync(
        GolfCartDbContext database,
        UserManager<GolfCartUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        await database.Database.EnsureCreatedAsync();

        if (!await roleManager.RoleExistsAsync(ApplicationRoles.Admin))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(ApplicationRoles.Admin));
            if (!roleResult.Succeeded) throw new InvalidOperationException("No se pudo crear el rol de administrador.");
        }

        if (!await database.Products.AnyAsync())
        {
            database.Products.AddRange(
                new GolfBallProduct { Brand = "Titleist", Grade = GolfBallGrade.MintA, PackSize = 12, Price = 24.95m, StockPacks = 8 },
                new GolfBallProduct { Brand = "Callaway", Grade = GolfBallGrade.GradeB, PackSize = 12, Price = 17.95m, StockPacks = 12 },
                new GolfBallProduct { Brand = "TaylorMade", Grade = GolfBallGrade.GradeC, PackSize = 12, Price = 12.95m, StockPacks = 6 },
                new GolfBallProduct { Brand = "Srixon", Grade = GolfBallGrade.GradeD, PackSize = 12, Price = 8.95m, StockPacks = 10 });
            await database.SaveChangesAsync();
        }

        var email = configuration["BootstrapAdmin:Email"];
        var password = configuration["BootstrapAdmin:Password"];
        if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(password)) return;
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            throw new InvalidOperationException("BootstrapAdmin requiere Email y Password.");

        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser is not null) return;

        var administrator = new GolfCartUser { UserName = email, Email = email, EmailConfirmed = true };
        var createResult = await userManager.CreateAsync(administrator, password);
        if (!createResult.Succeeded)
            throw new InvalidOperationException($"No se pudo crear el administrador: {string.Join(" ", createResult.Errors.Select(error => error.Description))}");

        var addRoleResult = await userManager.AddToRoleAsync(administrator, ApplicationRoles.Admin);
        if (!addRoleResult.Succeeded)
            throw new InvalidOperationException("No se pudo asignar el rol de administrador.");
    }
}

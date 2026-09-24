using GolfCart.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GolfCart.Data;

public class GolfCartDbContext(DbContextOptions<GolfCartDbContext> options) : IdentityDbContext<GolfCartUser>(options)
{
    public DbSet<GolfBallProduct> Products => Set<GolfBallProduct>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<GolfBallProduct>().Property(product => product.Price).HasPrecision(10, 2);
        modelBuilder.Entity<Order>().Property(order => order.Total).HasPrecision(10, 2);
        modelBuilder.Entity<OrderLine>().Property(line => line.UnitPrice).HasPrecision(10, 2);
        modelBuilder.Entity<OrderLine>().Property(line => line.LineTotal).HasPrecision(10, 2);
    }
}

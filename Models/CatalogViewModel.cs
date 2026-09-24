namespace GolfCart.Models;

public class CatalogViewModel
{
    public IReadOnlyList<GolfBallProduct> Products { get; init; } = [];
    public string? Brand { get; init; }
    public GolfBallGrade? Grade { get; init; }
    public IReadOnlyList<string> Brands { get; init; } = [];
}

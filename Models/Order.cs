namespace GolfCart.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public decimal Total { get; set; }
    public List<OrderLine> Lines { get; set; } = [];
}

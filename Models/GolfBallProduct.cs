using System.ComponentModel.DataAnnotations;

namespace GolfCart.Models;

public class GolfBallProduct
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Indica una marca.")]
    [StringLength(60)]
    [Display(Name = "Marca")]
    public string Brand { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Calidad")]
    public GolfBallGrade Grade { get; set; }

    [Range(1, 1000, ErrorMessage = "El lote debe contener al menos una bola.")]
    [Display(Name = "Bolas por lote")]
    public int PackSize { get; set; } = 12;

    [Range(typeof(decimal), "0.01", "9999", ErrorMessage = "Indica un precio válido.")]
    [Display(Name = "Precio por lote")]
    public decimal Price { get; set; }

    [Range(0, 100000, ErrorMessage = "El stock no puede ser negativo.")]
    [Display(Name = "Lotes disponibles")]
    public int StockPacks { get; set; }

    [Display(Name = "A la venta")]
    public bool IsActive { get; set; } = true;
}

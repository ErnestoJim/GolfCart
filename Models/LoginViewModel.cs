using System.ComponentModel.DataAnnotations;

namespace GolfCart.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Indica tu correo.")]
    [EmailAddress(ErrorMessage = "Indica un correo válido.")]
    [Display(Name = "Correo electrónico")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Indica tu contraseña.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Mantener sesión iniciada")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}

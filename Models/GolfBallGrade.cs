using System.ComponentModel.DataAnnotations;

namespace GolfCart.Models;

public enum GolfBallGrade
{
    [Display(Name = "Mint (A)")]
    MintA,
    [Display(Name = "Grado B")]
    GradeB,
    [Display(Name = "Grado C")]
    GradeC,
    [Display(Name = "Grado D")]
    GradeD
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ResortApplication.Domain.Entities;

public class VillaNumber
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "Villa Number")]
    public int VillaNumberId { get; set; }

    [Display(Name = "Villa")]
    [ForeignKey("Villa")]
    public int VillaId { get; set; }

    [ValidateNever] public Villa Villa { get; set; }

    public string? SpecialDetails { get; set; }
}
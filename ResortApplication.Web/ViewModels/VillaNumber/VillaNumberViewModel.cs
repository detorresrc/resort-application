using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VillaNumberEntity = ResortApplication.Domain.Entities.VillaNumber;

namespace ResortApplication.Web.ViewModels.VillaNumber;

public class VillaNumberViewModel
{
    public required VillaNumberEntity VillaNumber { get; set; }
    [ValidateNever] public IEnumerable<SelectListItem> Villas { get; set; } = [];
}
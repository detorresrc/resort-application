using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using AmenityEntity = ResortApplication.Domain.Entities.Amenity;
namespace ResortApplication.Web.ViewModels.Amenity;

public class AmenityViewModel
{
    public required AmenityEntity Amenity { get; set; }
    [ValidateNever] public IEnumerable<SelectListItem> Villas { get; set; } = [];
}
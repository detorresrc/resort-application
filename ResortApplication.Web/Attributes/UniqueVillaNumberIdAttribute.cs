using System.ComponentModel.DataAnnotations;
using ResortApplication.Infrastructure.Data;

namespace ResortApplication.Web.Attributes;

public class UniqueVillaNumberIdAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
    {
        var dbContext = validationContext.GetService(typeof(ApplicationDbContext)) as ApplicationDbContext;
        if (dbContext == null)
            throw new InvalidOperationException("DbContext not available");

        var villaNumberId = (int)value;
        var exists = dbContext.VillaNumbers.Any(v => v.VillaNumberId == villaNumberId);

        if (exists)
            return new ValidationResult("Villa Number ID must be unique.");

        return ValidationResult.Success;
    }
}
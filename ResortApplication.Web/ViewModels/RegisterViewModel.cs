using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ResortApplication.Web.ViewModels;

public class RegisterViewModel
{
    [Required]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    
    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; } = string.Empty;
    
    public string RedirectUrl { get; set; } = string.Empty;
    
    public string? Role { get; set; } = string.Empty;
    
    [ValidateNever]
    [Display(Name = "Select Role")]
    public IEnumerable<SelectListItem> RoleList { get; set; } = new List<SelectListItem>();
}
using System.ComponentModel.DataAnnotations;

namespace ResortApplication.Web.ViewModels;

public class LoginViewModel
{
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    
    public bool RememberMe { get; set; } = false;
    public string RedirectUrl { get; set; } = string.Empty;
}
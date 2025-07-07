using Microsoft.AspNetCore.Identity;

namespace ResortApplication.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
}
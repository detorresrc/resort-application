using ResortApplication.Domain.Entities;

namespace ResortApplication.Web.ViewModels;

public class HomeVillaChildViewModel
{
    public required HomeViewModel HomeViewModel { get; set; }
    public required Villa Villa { get; set; }
}
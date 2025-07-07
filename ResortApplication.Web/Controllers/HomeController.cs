using Microsoft.AspNetCore.Mvc;
using ResortApplication.Application.Common.Interfaces;
using ResortApplication.Web.ViewModels;

namespace ResortApplication.Web.Controllers;

public class HomeController(IUnitOfWork unitOfWork) : Controller
{
    

    public async Task<IActionResult> Index()
    {
        HomeViewModel vm = new()
        {
            VillaList = await unitOfWork.Villa.GetAllAsync(filter: null, includeProperties: "Amenities"),
            NumberOfNights = 1,
            CheckInDate = DateOnly.FromDateTime(DateTime.Now)
        };
        
        return View(vm);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Error()
    {
        return View();
    }
}
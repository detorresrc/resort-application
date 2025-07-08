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

    [HttpPost]
    public async Task<IActionResult> Index(HomeViewModel vm)
    {
        vm.VillaList = await unitOfWork.Villa.GetAllAsync(filter: null, includeProperties: "Amenities"); //TODO: filter by dates

        foreach (var villa in vm.VillaList)
        {
            //TODO: check availability based on dates
            if (villa.Id % 2 == 0)
                villa.IsAvailable = false;
        }
        
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> GetVillasByDate(int nights, DateOnly checkInDate)
    {
        var villaList = await unitOfWork.Villa.GetAllAsync(filter: null, includeProperties: "Amenities"); //TODO: filter by dates

        foreach (var villa in villaList)
        {
            //TODO: check availability based on dates
            if (villa.Id % 2 == 0)
                villa.IsAvailable = false;
        }

        return PartialView("_VillaListPartial", new HomeViewModel()
        {
            VillaList = villaList,
            CheckInDate = checkInDate,
            NumberOfNights = nights
        });
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
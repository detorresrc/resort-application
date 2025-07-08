using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ResortApplication.Application.Common;
using ResortApplication.Application.Common.Interfaces;
using ResortApplication.Domain.Entities;
using ResortApplication.Web.ViewModels.Amenity;

namespace ResortApplication.Web.Controllers;

[Authorize(Roles = SD.RoleAdmin)]
public class AmenityController(IUnitOfWork unitOfWork) : Controller
{
    public async Task<IActionResult> Index()
    {
        var amenities = await unitOfWork.Amenity
            .GetAllAsync(null, "Villa", true);
        return View(amenities);
    }
    
    public async Task<IActionResult> Create()
    {
        return View(new AmenityViewModel
        {
            Amenity = new Amenity(),
            Villas = await GetVillaForDropDownList()
        });
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(AmenityViewModel vm)
    {
        if (ModelState.IsValid)
        {
            unitOfWork.Amenity.Add(vm.Amenity);
            await unitOfWork.SaveAsync();
            TempData["success"] = "Amenity created successfully";
            return RedirectToAction(nameof(Index));
        }

        vm.Villas = await GetVillaForDropDownList();

        return View(vm);
    }
    
    [HttpGet]
    [Route("[controller]/[action]/{amenityId:int}")]
    public async Task<IActionResult> Update(int amenityId)
    {
        var amenity = await unitOfWork.Amenity.GetAsync(v => v.Id == amenityId, "Villa", true);

        if (amenity is null)
            return RedirectToAction("Error", "Home");

        return View(new AmenityViewModel
        {
            Amenity = amenity,
            Villas = await GetVillaForDropDownList()
        });
    }
    
    [HttpPost]
    [Route("[controller]/[action]/{amenityId:int}")]
    public async Task<IActionResult> Update(int amenityId, AmenityViewModel vm)
    {
        if (ModelState.IsValid is false)
            return View(vm);

        unitOfWork.Amenity.Update(vm.Amenity);
        await unitOfWork.SaveAsync();
        TempData["success"] = "Amenity updated successfully";
        return RedirectToAction(nameof(Update), "Amenity", new { amenityId });
    }
    
    [HttpGet]
    [Route("[controller]/[action]/{amenityId:int}")]
    public async Task<IActionResult> Delete(int amenityId)
    {
        var amenity = await unitOfWork.Amenity.GetAsync(v => v.Id == amenityId, "Villa", true);
        if (amenity is not null)
            return View(new AmenityViewModel
            {
                Amenity = amenity,
                Villas = []
            });

        TempData["error"] = "Amenity not found";
        return RedirectToAction("Error", "Home");
    }
    
    [HttpPost]
    [Route("[controller]/[action]/{amenityId:int}")]
    public async Task<IActionResult> Delete(int amenityId, AmenityViewModel vm)
    {
        var amenity = await unitOfWork.Amenity.GetAsync(v => v.Id == amenityId, "Villa", true);
        if (amenity is null)
        {
            TempData["error"] = "Amenity not found";
            return RedirectToAction("Error", "Home");
        }

        unitOfWork.Amenity.Delete(amenity);
        await unitOfWork.SaveAsync();
        TempData["success"] = "Amenity deleted successfully";
        return RedirectToAction(nameof(Index));
    }
    
    private async Task<IEnumerable<SelectListItem>> GetVillaForDropDownList()
    {
        return (await unitOfWork.Villa.GetAllAsync(null, null, true))
            .Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }).ToList();
    }
}
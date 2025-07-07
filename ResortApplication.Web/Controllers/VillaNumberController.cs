using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ResortApplication.Application.Common.Interfaces;
using ResortApplication.Domain.Entities;
using ResortApplication.Web.ViewModels.VillaNumber;

namespace ResortApplication.Web.Controllers;

public class VillaNumberController(IUnitOfWork unitOfWork) : Controller
{
    public async Task<IActionResult> Index()
    {
        var villaNumbers = await unitOfWork.VillaNumber.GetAllAsync(null, "Villa", true);

        return View(villaNumbers);
    }

    public async Task<IActionResult> Create()
    {
        return View(new VillaNumberViewModel
        {
            VillaNumber = new VillaNumber
            {
                SpecialDetails = "Please add special details here"
            },
            Villas = await GetVillaForDropDownList()
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create(VillaNumberViewModel villaNumberViewModel)
    {
        var isVillaNumberIdExist =
            (await unitOfWork.VillaNumber.GetAllAsync(
                v => v.VillaNumberId == villaNumberViewModel.VillaNumber.VillaNumberId,
                null, true)).Any();

        if (isVillaNumberIdExist)
            ModelState.AddModelError("VillaNumber.VillaNumberId", "Villa Number ID must be unique.");

        if (ModelState.IsValid)
        {
            unitOfWork.VillaNumber.Add(villaNumberViewModel.VillaNumber);
            await unitOfWork.SaveAsync();
            TempData["success"] = "Villa Number created successfully";
            return RedirectToAction(nameof(Index));
        }

        villaNumberViewModel.Villas = await GetVillaForDropDownList();

        return View(villaNumberViewModel);
    }


    [HttpGet]
    [Route("[controller]/[action]/{villaNumberId:int}")]
    public async Task<IActionResult> Update(int villaNumberId)
    {
        var villaNumber = await unitOfWork.VillaNumber.GetAsync(v => v.VillaNumberId == villaNumberId, "Villa", true);

        if (villaNumber is null)
            return RedirectToAction("Error", "Home");

        return View(new VillaNumberViewModel
        {
            VillaNumber = villaNumber,
            Villas = await GetVillaForDropDownList()
        });
    }

    [HttpPost]
    [Route("[controller]/[action]/{villaNumberId:int}")]
    public async Task<IActionResult> Update(int villaNumberId, VillaNumberViewModel villaNumberViewModel)
    {
        var isVillaNumberIdExist =
            (await unitOfWork.VillaNumber.GetAllAsync(v =>
                    v.VillaNumberId == villaNumberViewModel.VillaNumber.VillaNumberId &&
                    v.VillaNumberId != villaNumberId,
                null, true)).Any();

        if (isVillaNumberIdExist)
            ModelState.AddModelError("VillaNumber.VillaNumberId", "Villa Number ID must be unique.");

        if (ModelState.IsValid is false)
            return View(villaNumberViewModel);

        unitOfWork.VillaNumber.Update(villaNumberViewModel.VillaNumber);
        await unitOfWork.SaveAsync();
        TempData["success"] = "Villa Number updated successfully";
        return RedirectToAction(nameof(Update), "VillaNumber", new { villaNumberId });
    }

    [HttpGet]
    [Route("[controller]/[action]/{villaNumberId:int}")]
    public async Task<IActionResult> Delete(int villaNumberId)
    {
        var villaNumber = await unitOfWork.VillaNumber.GetAsync(v => v.VillaNumberId == villaNumberId, "Villa", true);
        if (villaNumber is not null)
            return View(new VillaNumberViewModel
            {
                VillaNumber = villaNumber,
                Villas = []
            });

        TempData["error"] = "Villa Number not found";
        return RedirectToAction("Error", "Home");
    }

    [HttpPost]
    [Route("[controller]/[action]/{villaNumberId:int}")]
    public async Task<IActionResult> Delete(int villaNumberId, VillaNumberViewModel villaNumberViewModel)
    {
        var villaNumber = await unitOfWork.VillaNumber.GetAsync(v => v.VillaNumberId == villaNumberId, "Villa", true);
        if (villaNumber is null)
        {
            TempData["error"] = "Villa Number not found";
            return RedirectToAction("Error", "Home");
        }

        unitOfWork.VillaNumber.Delete(villaNumber);
        await unitOfWork.SaveAsync();
        TempData["success"] = "Villa Number deleted successfully";
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
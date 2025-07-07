using Microsoft.AspNetCore.Mvc;
using ResortApplication.Application.Common.Interfaces;
using ResortApplication.Domain.Entities;

namespace ResortApplication.Web.Controllers;

public class VillaController(IUnitOfWork unitOfWork) : Controller
{
    public async Task<IActionResult> Index()
    {
        var villas = await unitOfWork.Villa.GetAllAsync(null, null, true);

        return View(villas);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Villa villa)
    {
        if (villa.Name == villa.Description)
            ModelState.AddModelError("Name", "The Villa Name cannot be the same as the Description");

        if (!ModelState.IsValid) return View();

        unitOfWork.Villa.Add(villa);
        await unitOfWork.SaveAsync();
        TempData["success"] = "Villa created successfully";
        return RedirectToAction("Index", "Villa");
    }

    [HttpGet]
    [Route("[controller]/[action]/{villaId:int}")]
    public async Task<IActionResult> Update(int villaId)
    {
        var villa = await unitOfWork.Villa.GetAsync(v => v.Id == villaId);
        if (villa == null) return RedirectToAction("Error", "Home");

        return View(villa);
    }

    [HttpPost]
    [Route("[controller]/[action]/{villaId:int}")]
    public async Task<IActionResult> Update(int villaId, Villa villa)
    {
        if (villa.Name == villa.Description)
            ModelState.AddModelError("Name", "The Villa Name cannot be the same as the Description");

        if (!ModelState.IsValid) return View();

        unitOfWork.Villa.Update(villa);
        await unitOfWork.SaveAsync();
        TempData["success"] = "Villa updated successfully";
        return RedirectToAction("Update", "Villa", new { villaId = villa.Id });
    }

    [HttpGet]
    [Route("[controller]/[action]/{villaId:int}")]
    public async Task<IActionResult> Delete(int villaId)
    {
        var villa = await unitOfWork.Villa.GetAsync(v => v.Id == villaId);
        if (villa == null) return RedirectToAction("Error", "Home");
        return View(villa);
    }

    [HttpPost]
    [Route("[controller]/[action]/{villaId:int}")]
    public async Task<IActionResult> Delete(int villaId, Villa villa)
    {
        var villaToDelete = await unitOfWork.Villa.GetAsync(v => v.Id == villaId, null, true);
        if (villaToDelete is null) return RedirectToAction("Error", "Home");

        unitOfWork.Villa.Delete(villa);
        await unitOfWork.SaveAsync();
        TempData["success"] = "Villa deleted successfully";
        return RedirectToAction("Index", "Villa");
    }
}
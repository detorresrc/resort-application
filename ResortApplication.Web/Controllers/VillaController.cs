using Microsoft.AspNetCore.Mvc;
using ResortApplication.Application.Common.Interfaces;
using ResortApplication.Domain.Entities;

namespace ResortApplication.Web.Controllers;

public class VillaController(
    IUnitOfWork unitOfWork,
    IWebHostEnvironment webHostEnvironment) : Controller
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

        if (villa.Image is not null)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(villa.Image.FileName);
            var imagePath = Path.Combine(webHostEnvironment.WebRootPath, "images", "Villa");

            await using var fs = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
            await villa.Image.CopyToAsync(fs);

            villa.ImageUrl = $"/images/Villa/{fileName}";
        }
        else
        {
            villa.ImageUrl = "https://placehold.co/600x400";
        }

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

        if (villa.Image is not null)
        {
            var fileName = Guid.NewGuid() + Path.GetExtension(villa.Image.FileName);
            var imagePath = Path.Combine(webHostEnvironment.WebRootPath, "images", "Villa");

            if (!string.IsNullOrEmpty(villa.ImageUrl))
            {
                var oldImage = Path.Combine(webHostEnvironment.WebRootPath, villa.ImageUrl.TrimStart('\\').TrimStart('/'));
                if (System.IO.File.Exists(oldImage)) System.IO.File.Delete(oldImage);
            }

            await using var fs = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
            await villa.Image.CopyToAsync(fs);

            villa.ImageUrl = $"/images/Villa/{fileName}";
        }

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
        
        if (!string.IsNullOrEmpty(villaToDelete.ImageUrl))
        {
            var oldImage = Path.Combine(webHostEnvironment.WebRootPath, villaToDelete.ImageUrl.TrimStart('\\').TrimStart('/'));
            if (System.IO.File.Exists(oldImage)) System.IO.File.Delete(oldImage);
        }
        
        TempData["success"] = "Villa deleted successfully";
        return RedirectToAction("Index", "Villa");
    }
}
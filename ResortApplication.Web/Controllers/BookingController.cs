using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResortApplication.Application.Common.Interfaces;
using ResortApplication.Domain.Entities;

namespace ResortApplication.Web.Controllers;

public class BookingController(
    IUnitOfWork unitOfWork
    ) : Controller
{
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> FinalizeBooking(
        int villaId,
        DateOnly checkInDate,
        int nights
        )
    {
        var appUser = await unitOfWork.ApplicationUser.GetAsync(u => u.Id == GetUserId());
        if(appUser is null)
        {
            TempData["error"] = "User not found.";
            return RedirectToAction("Index", "Home");
        }
        
        var villa = await unitOfWork.Villa.GetAsync(v => v.Id == villaId, "Amenities");
        if (villa is null)
        {
            TempData["error"] = "Villa not found.";
            return RedirectToAction("Index", "Home");
        }
        
        Booking booking = new()
        {
            VillaId = villaId,
            Villa = villa,
            CheckInDate = checkInDate,
            CheckOutDate = checkInDate.AddDays(nights),
            Nights = nights,
            TotalCost = villa.Price * nights,
            Name = appUser.Name,
            Email = appUser.Email ?? string.Empty,
            PhoneNumber = appUser.PhoneNumber,
            UserId = GetUserId()
        };
        
        return View(booking);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> FinalizeBooking(
        int villaId,
        DateOnly checkInDate,
        int nights,
        Booking booking)
    {
        var villa = await unitOfWork.Villa.GetAsync(v => v.Id == booking.VillaId, "Amenities");
        if (villa is null)
        {
            TempData["error"] = "Villa not found.";
            return RedirectToAction("Index", "Home");
        }

        if (villa.IsAvailable is false)
        {
            TempData["error"] = "Villa is not available for booking.";
            return RedirectToAction("Index", "Home");
        }

        booking.UserId = GetUserId();
        booking.CheckInDate = checkInDate;
        booking.CheckOutDate = checkInDate.AddDays(nights);
        booking.Nights = nights;
        booking.Status = BookingStatus.Pending;
        booking.TotalCost = villa.Price * booking.Nights;
        unitOfWork.Booking.Add(booking);
        await unitOfWork.SaveAsync();
        
        return RedirectToAction(nameof(BookingConfirmation), new { bookingId = booking.Id });
    }

    [Authorize]
    public IActionResult BookingConfirmation(int bookingId)
    {
        return View(bookingId);
    }
    
    private string GetUserId()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity!;
        return claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    }
}
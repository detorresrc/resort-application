using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResortApplication.Application.Common.Interfaces;
using ResortApplication.Domain.Entities;
using Stripe.Checkout;
using Session = Stripe.Checkout.Session;
using SessionService = Stripe.Checkout.SessionService;

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
        if (appUser is null)
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
        booking.BookingDate = DateOnly.FromDateTime(DateTime.Now);
        booking.Nights = nights;
        booking.Status = BookingStatus.Pending;
        booking.TotalCost = villa.Price * booking.Nights;
        unitOfWork.Booking.Add(booking);
        await unitOfWork.SaveAsync();

        var session = await CreateStripeSession(booking, villa);

        unitOfWork.Booking.UpdateStripePaymentId(booking, session.Id);
        await unitOfWork.SaveAsync();
        
        Response.Headers.Append("Location", session.Url);
        return StatusCode(303);
    }

    private Task<Session> CreateStripeSession(
        Booking booking,
        Villa villa
    )
    {
        var domain = Request.Scheme + "://" + Request.Host.Value + "/";
        var options = new SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>(),
            Mode = "payment",
            SuccessUrl = domain + $"booking/BookingConfirmation?bookingId={booking.Id}",
            CancelUrl = domain +
                        $"booking/FinalizeBooking?villaId={villa.Id}&checkInDate={booking.CheckInDate}&nights={booking.Nights}",
            CustomerEmail = booking.Email
        };

        options.LineItems.Add(new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions
            {
                UnitAmount = (long)(booking.TotalCost * 100), // Convert to cents
                Currency = "usd",
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = villa.Name,
                    Description = villa.Description,
                    Images = [domain + villa.ImageUrl]
                }
            },
            Quantity = 1
        });

        var service = new SessionService();
        return service.CreateAsync(options);
    }

    [Authorize]
    public async Task<IActionResult> BookingConfirmation(int bookingId)
    {
        var booking = await unitOfWork.Booking.GetAsync(b => b.Id == bookingId, "Villa,User");
        if(booking is null)
        {
            TempData["error"] = "Booking not found.";
            return RedirectToAction("Index", "Home");
        }

        if (booking.Status == BookingStatus.Pending)
        {
            var service  = new SessionService();
            var session = await service.GetAsync(booking.StripeSessionId);
            if (session.PaymentStatus == "paid")
            {
                unitOfWork.Booking.UpdateStatus(booking, BookingStatus.Approved);
                unitOfWork.Booking.UpdateStripePaymentId(booking, booking.StripeSessionId!, session.PaymentIntentId);
                await unitOfWork.SaveAsync();
            }
        }
        
        return View(booking);
    }

    private string GetUserId()
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity!;
        return claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    }
}
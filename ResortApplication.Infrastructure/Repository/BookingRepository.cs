using Microsoft.EntityFrameworkCore;
using ResortApplication.Application.Common.Exception;
using ResortApplication.Application.Common.Interfaces;
using ResortApplication.Domain.Entities;
using ResortApplication.Infrastructure.Data;

namespace ResortApplication.Infrastructure.Repository;

public class BookingRepository(ApplicationDbContext db) : Repository<Booking>(db), IBookingRepository
{
    public async Task<Booking> UpdateStatus(int bookingId, BookingStatus status)
    {
        var booking = await db.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId);
        if (booking is null)
            throw new BookingNotFoundException("Booking not found with id: " + bookingId);

        UpdateStatus(booking, status);

        await db.SaveChangesAsync();

        return booking;
    }

    public async Task<Booking> UpdateStripePaymentId(int bookingId, string sessionId, string? paymentIntentId=null)
    {
        var booking = await db.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId);
        if (booking is null)
            throw new BookingNotFoundException("Booking not found with id: " + bookingId);

        UpdateStripePaymentId(booking, sessionId, paymentIntentId);
        
        await db.SaveChangesAsync();
        return booking;
    }

    public Booking UpdateStatus(Booking booking, BookingStatus status)
    {
        booking.Status = status;
        switch (booking.Status)
        {
            case BookingStatus.Checkedin:
                booking.CheckInDate = DateOnly.FromDateTime(DateTime.UtcNow);
                break;
            case BookingStatus.Completed:
                booking.CheckOutDate = DateOnly.FromDateTime(DateTime.UtcNow);
                break;
        }

        return booking;
    }

    public Booking UpdateStripePaymentId(Booking booking, string sessionId, string? paymentIntentId = null)
    {
        if(!string.IsNullOrEmpty(sessionId))
            booking.StripeSessionId = sessionId;

        if (string.IsNullOrEmpty(paymentIntentId)) return booking;
        
        booking.StripePaymentIntentId = paymentIntentId;
        booking.PaymentDate = DateTime.Now;
        booking.IsPaymentCompleted = true;

        return booking;
    }
}
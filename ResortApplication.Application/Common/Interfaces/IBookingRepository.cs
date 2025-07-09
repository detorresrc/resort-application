using ResortApplication.Domain.Entities;

namespace ResortApplication.Application.Common.Interfaces;

public interface IBookingRepository : IRepository<Booking>
{
    Task<Booking> UpdateStatus(int bookingId, BookingStatus status);
    Task<Booking> UpdateStripePaymentId(int bookingId, string sessionId, string? paymentIntentId = null);
    
    Booking UpdateStatus(Booking booking, BookingStatus status);
    Booking UpdateStripePaymentId(Booking booking, string sessionId, string? paymentIntentId = null);
}
namespace ResortApplication.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IVillaRepository Villa { get; }
    IVillaNumberRepository VillaNumber { get; }
    IAmenityRepository Amenity { get; }
    IBookingRepository Booking { get; }
    IApplicationUserRepository ApplicationUser { get; }
    Task<int> SaveAsync();
}
using ResortApplication.Application.Common.Interfaces;
using ResortApplication.Infrastructure.Data;

namespace ResortApplication.Infrastructure.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;

    public UnitOfWork(ApplicationDbContext db)
    {
        _db = db;
        Villa = new VillaRepository(_db);
        VillaNumber = new VillaNumberRepository(_db);
        Amenity = new AmenityRepository(_db);
        Booking = new BookingRepository(_db);
        ApplicationUser = new ApplicationUserRepository(_db);
    }

    public IVillaRepository Villa { get; }
    public IVillaNumberRepository VillaNumber { get; }
    public IAmenityRepository Amenity { get; }
    public IBookingRepository Booking { get;  }
    public IApplicationUserRepository ApplicationUser { get; }
    public async Task<int> SaveAsync()
    {
        return await _db.SaveChangesAsync();
    }
}
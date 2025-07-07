namespace ResortApplication.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IVillaRepository Villa { get; }
    IVillaNumberRepository VillaNumber { get; }
    IAmenityRepository Amenity { get; }
    Task<int> SaveAsync();
}
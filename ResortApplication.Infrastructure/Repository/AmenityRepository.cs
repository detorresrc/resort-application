using ResortApplication.Application.Common.Interfaces;
using ResortApplication.Domain.Entities;
using ResortApplication.Infrastructure.Data;

namespace ResortApplication.Infrastructure.Repository;

public class AmenityRepository(ApplicationDbContext db) : Repository<Amenity>(db), IAmenityRepository
{
}
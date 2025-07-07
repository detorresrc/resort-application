using ResortApplication.Application.Common.Interfaces;
using ResortApplication.Domain.Entities;
using ResortApplication.Infrastructure.Data;

namespace ResortApplication.Infrastructure.Repository;

public class VillaNumberRepository(ApplicationDbContext db) : Repository<VillaNumber>(db), IVillaNumberRepository
{
}
using System.Security.Claims;
using ResortApplication.Application.Common.Interfaces;
using ResortApplication.Domain.Entities;
using ResortApplication.Infrastructure.Data;

namespace ResortApplication.Infrastructure.Repository;

public class ApplicationUserRepository(ApplicationDbContext db) : Repository<ApplicationUser>(db), IApplicationUserRepository
{
}
using ResortApplication.Application.Common.Interfaces;
using ResortApplication.Infrastructure.Repository;

namespace ResortApplication.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
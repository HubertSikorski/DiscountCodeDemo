using DiscountCodeDemo.Core.Interfaces;
using DiscountCodeDemo.Core.Services;
using DiscountCodeDemo.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DiscountCodeDemo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string jsonFilePath)
    {
        services.AddSingleton<IDiscountCodeRepository>(provider =>
            new JsonFileDiscountCodeRepository(jsonFilePath));
        
        services.AddScoped<IDiscountCodeService, DiscountCodeService>();
        
        return services;
    }
}
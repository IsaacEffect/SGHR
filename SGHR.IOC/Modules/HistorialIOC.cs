using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGHR.Application.Contracts.Service;
using SGHR.Application.Services;
using SGHR.Domain.Interfaces.Repository;
using SGHR.Persistence.Repositories;

namespace SGHR.IOC.Modules
{
    public static class HistorialIOC
    {
        public static void RegisterHistorial(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IHistorialReservaRepository, HistorialReservaRepository>();
            services.AddScoped<IHistorialReservaService, HistorialReservaService>();
        }
    }
}

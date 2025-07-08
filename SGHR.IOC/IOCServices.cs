using Microsoft.Extensions.DependencyInjection;
using SGHR.Application.Interfaces;
using SGHR.Application.Services;
using SGHR.Persistence.Interfaces;
using SGHR.Persistence.Repositories;

namespace SGHR.IOC
{
    public static class IOCServices
    {
        public static void Register(IServiceCollection services)
        {
            // Servicios de aplicación
            services.AddScoped<IPisoService, PisoService>();

            // Repositorios
            services.AddScoped<IPisoRepository, PisoRepository>();
        }
    }
}

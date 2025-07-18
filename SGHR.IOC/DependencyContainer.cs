using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGHR.Domain.Base;
using SGHR.Domain.Interfaces;
using SGHR.IOC.Modules;
using SGHR.Persistence;
using SGHR.Persistence.Base;

namespace SGHR.IOC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterClientes(configuration);
            services.RegisterHistorial(configuration);
            services.RegisterDbContext(configuration);
            services.AddScoped<ISqlHelper, SqlHelperService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddLogging();
        }
    }
}

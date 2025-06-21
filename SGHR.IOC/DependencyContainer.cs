using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGHR.Domain.Interfaces;
using SGHR.Domain.Repository;
using SGHR.Persistence;
using SGHR.Persistence.Context;
using SGHR.Persistence.Repositories;

namespace SGHR.IOC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IHistorialReservaRepository, HistorialReservaRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddLogging();
            //

            var connectionString = configuration.GetConnectionString("HotelDBConnection");
            services.AddDbContext<HotelReservaDBContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}

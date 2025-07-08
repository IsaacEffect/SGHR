using Microsoft.Extensions.DependencyInjection;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Application.Interfaces.Servicios;
using SGHR.Application.Services.Reservas;
using SGHR.Application.Services.Servicios;
using SGHR.Domain.Interfaces;
using SGHR.Persistence.Interfaces.Repositories.Clientes;
using SGHR.Persistence.Interfaces.Repositories.Habitaciones;
using SGHR.Persistence.Interfaces.Repositories.Reservas;
using SGHR.Persistence.Interfaces.Repositories.Servicios;
using SGHR.Persistence.Repositories.Clientes;
using SGHR.Persistence.Repositories.Habitaciones;
using SGHR.Persistence.Repositories.Reservas;
using SGHR.Persistence.Repositories.Servicios;
using SGHR.Persistence.UnitOfWork;

namespace SGHR.IOC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services)
        {
            // Application Services 
            services.AddScoped<IReservaApplicationService, ReservaApplicationService>();
            services.AddScoped<IServicioApplicationService, ServicioApplicationService>();

            // Repositories
            services.AddScoped<IReservaRepository, ReservaRepository>();
            services.AddScoped<ICategoriaHabitacionRepository, CategoriaHabitacionRepository>();
            services.AddScoped<IServicioRepository, ServicioRepository>();
            services.AddScoped<IServicioCategoriaRepository, ServicioCategoriaRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();

            // UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Validations
            

            return services;
        }

    }
}

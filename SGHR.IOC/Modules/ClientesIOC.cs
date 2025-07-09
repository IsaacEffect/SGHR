using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGHR.Application.Contracts.Service;
using SGHR.Application.Services;
using SGHR.Domain.Interfaces.Repository;

namespace SGHR.IOC.Modules
{
    public static class ClientesIOC
    {
        public static void RegisterClientes(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
        }
    }
}

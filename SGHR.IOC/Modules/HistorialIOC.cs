using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGHR.Application.Services;
using SGHR.Domain.Interfaces.Repository;
using SGHR.Domain.Interfaces.Service;
using SGHR.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

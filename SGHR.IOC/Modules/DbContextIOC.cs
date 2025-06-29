using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SGHR.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGHR.IOC.Modules
{
    public static class DbContextIOC
    {
        public static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("HotelDBConnection");

            services.AddDbContext<HotelReservaDBContext>(options =>
                options.UseSqlServer(connectionString));
        }
    }
}

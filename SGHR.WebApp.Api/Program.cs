using Microsoft.EntityFrameworkCore;

using SGHR.Application.Services.Servicios;
using SGHR.Application.Interfaces.Servicios;
using SGHR.Application.Interfaces.Reservas;
using SGHR.Application.Services.Reservas;

using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces;
using SGHR.Persistence.Interfaces.Repositories.Habitaciones;
using SGHR.Persistence.Interfaces.Repositories.Reservas;
using SGHR.Persistence.Interfaces.Repositories.Servicios;
using SGHR.Persistence.Interfaces.Repositories.Clientes;
using SGHR.Persistence.Repositories.Habitaciones;
using SGHR.Persistence.Repositories.Reservas;
using SGHR.Persistence.Repositories.Servicios;
using SGHR.Persistence.Repositories.Clientes;

namespace SGHR.WebApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.  
            builder.Services.AddDbContext<SGHRDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("SGHR")));


            // Repositories
            builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
            builder.Services.AddScoped<ICategoriaHabitacionRepository, CategoriaHabitacionRepository>();
            builder.Services.AddScoped<IServicioRepository, ServicioRepository>();
            builder.Services.AddScoped<IServicioCategoriaRepository, ServicioCategoriaRepository>();
            builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
            
            // Application Services
             builder.Services.AddScoped<IServicioApplicationService, ServicioApplicationService>();
             builder.Services.AddScoped<IReservaApplicationService, ReservaApplicationService>();

            // AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

           

            builder.Services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();

            

            var app = builder.Build();

            // Configure the HTTP request pipeline.  
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
            
}

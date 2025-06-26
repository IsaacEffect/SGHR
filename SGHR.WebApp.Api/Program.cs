using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces;
using SGHR.Persistence.Interfaces.Repositories.Habitaciones;
using SGHR.Persistence.Interfaces.Repositories.Reservas;
using SGHR.Persistence.Interfaces.Repositories.Servicios;
using SGHR.Persistence.Repositories.Habitaciones;
using SGHR.Persistence.Repositories.Reservas;
using SGHR.Persistence.Repositories.Servicios;
namespace SGHR.WebApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.  

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle  
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<SGHRDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SGHR")));
        
            builder.Services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();

            // Application Services
            // builder.Services.AddScoped<IServicioApplicationService, ServicioApplicationService>();
            // builder.Services.AddScoped<IReservaApplicationService, ReservaApplicationService>();


            // Repositories
            builder.Services.AddScoped<IReservaRepository,ReservaRepository>();
            builder.Services.AddScoped<ICategoriaHabitacionRepository, CategoriaHabitacionRepository>();
            builder.Services.AddScoped<IServicioRepository, ServicioRepository>();
            builder.Services.AddScoped<IServicioCategoriaRepository, ServicioCategoriaRepository>();
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

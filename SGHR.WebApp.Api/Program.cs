
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SGHR.Application.InterfacesServices;
using SGHR.Application.ServicesApplication;
using SGHR.Domain.InterfacesDomainServices;
using SGHR.Domain.InterfacesRepositories;
using SGHR.Domain.InterfacesServices;
using SGHR.Persistance.DBContext;
using SGHR.Persistance.Repositories;
using SGHR.Domain.DomainServices;
using SGHR.Infraestructure.Common.StoredProcedures;

namespace SGHR.WebApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configurar DbContext
            builder.Services.AddDbContext<SGHRDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 
            builder.Services.AddScoped<ITarifaRepository, TarifaRepository>();
            builder.Services.AddScoped<ITarifaService, TarifaService>();
            builder.Services.AddScoped<ICategoriasHabitacionRepository, CategoriasHabitacionRepository>();
            builder.Services.AddScoped<ICategoriasHabitacionesService, CategoriasHabitacioneService>();
            builder.Services.AddScoped<ICategoriasHabitacionesDomainService, CategoriasHabitacionDomainService>();
            builder.Services.AddScoped<IHabitacionesRepository, HabitacionesRepository>();
           


            // Add services to the container.



            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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


namespace SGHR.WebApp.Api 
{ 
    using Microsoft.EntityFrameworkCore;
using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces;
using SGHR.Persistence.Repositories;
    using SGHR.IOC;


    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

        // Registrar el DbContext
        builder.Services.AddDbContext<SGHRContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Registrar el repositoro
        builder.Services.AddScoped<IPisoRepository, PisoRepository>();

        

        builder.Services.AddControllers();
            
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.RegisterServices(); 


            var app = builder.Build();

            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SGHRContext>();
                context.Database.EnsureCreated(); 
            }


            app.Run();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces;

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

            // Fix for CS1061: Ensure the Microsoft.EntityFrameworkCore.SqlServer package is installed  
            builder.Services.AddDbContext<SGHRDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SGHR")));

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

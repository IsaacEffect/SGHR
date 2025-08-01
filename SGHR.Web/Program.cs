using SGHR.Web.ApiRepositories;
using SGHR.Web.ApiRepositories.Interfaces.Reservas;
using SGHR.Web.ApiRepositories.Interfaces.Servicios;
using SGHR.Web.ApiServices.Interfaces;
using SGHR.Web.ApiServices;
using SGHR.Web.ViewModel.Mapping.Servicios;
namespace SGHR.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            // ApiServices
            builder.Services.AddScoped<IServicioApiService, ServiciosApiService>();
            // repositories
            builder.Services.AddHttpClient<IReservasApiRepository, ReservasApiRepository>(client =>
            {
                var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];
                if (string.IsNullOrEmpty(baseUrl))
                {
                    throw new InvalidOperationException("La URL base de la API no está configurada. Por favor, añade 'ApiSettings:BaseUrl' en appsettings.json.");
                }
                client.BaseAddress = new Uri(baseUrl);
            });
            builder.Services.AddHttpClient<IServiciosApiRepository,ServiciosApiRepository>(client =>
            {
                var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];
                if (string.IsNullOrEmpty(baseUrl))
                {
                    throw new InvalidOperationException("La URL base de la API no está configurada. Por favor, añade 'ApiSettings:BaseUrl' en appsettings.json.");
                }
                client.BaseAddress = new Uri(baseUrl);
            });
            builder.Services.AddHttpClient<IServicioCategoriaApiRepository, ServicioCategoriaApiRepository>(client =>
            {
                var baseUrl = builder.Configuration["ApiSettings:BaseUrl"];
                if (string.IsNullOrEmpty(baseUrl))
                {
                    throw new InvalidOperationException("La URL base de la API no está configurada. Por favor, añade 'ApiSettings:BaseUrl' en appsettings.json.");
                }
                client.BaseAddress = new Uri(baseUrl);
            });
            // Mapper
            builder.Services.AddAutoMapper(typeof(ServicioProfileApi));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

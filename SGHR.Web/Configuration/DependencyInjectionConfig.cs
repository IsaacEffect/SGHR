using SGHR.Web.ApiRepositories.Interfaces.Reservas;
using SGHR.Web.ApiRepositories.Interfaces.Servicios;
using SGHR.Web.ApiRepositories.Reservas;
using SGHR.Web.ApiRepositories.Servicios;
using SGHR.Web.ApiServices.Interfaces.Reservas;
using SGHR.Web.ApiServices.Interfaces.Servicios;
using SGHR.Web.ApiServices.Reservas;
using SGHR.Web.ApiServices.Servicios;
using SGHR.Web.ViewModel.Mapping.Reservas;
using SGHR.Web.ViewModel.Mapping.Servicios;
using SGHR.Web.ViewModel.Presenters;
using SGHR.Web.ViewModel.Presenters.Interfaces;

namespace SGHR.Web.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            // Controllers
            services.AddControllersWithViews();

            // ApiServices
            services.AddScoped<IServicioApiService, ServiciosApiService>();
            services.AddScoped<IReservasApiService, ReservasApiService>();

            // Repositories
            var baseUrl = configuration["ApiSettings:BaseUrl"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new InvalidOperationException("La URL base de la API no está configurada. Por favor, añade 'ApiSettings:BaseUrl' en appsettings.json.");
            }

            services.AddHttpClient<IReservasApiRepository, ReservasApiRepository>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            });

            services.AddHttpClient<IServiciosApiRepository, ServiciosApiRepository>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            });

            services.AddHttpClient<IServicioCategoriaApiRepository, ServicioCategoriaApiRepository>(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            });

            // AutoMapper
            services.AddAutoMapper(typeof(ServicioProfileApi));
            services.AddAutoMapper(typeof(ReservasProfileApi));

            // Presenters
            services.AddScoped<IReservasPresenter, ReservasPresenter>();
        }
    }
}
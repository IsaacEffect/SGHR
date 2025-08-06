using SGHR.Web.Controllers;
using SGHR.Web.Helpers;
using SGHR.Web.Helpers.Abstraction;

namespace SGHR.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //
            builder.Services.AddScoped<IHelper, Helper>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpClient<CategoriasHabitacionController>();


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

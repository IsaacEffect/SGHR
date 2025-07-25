using Microsoft.EntityFrameworkCore;
using SGHR.Application.Interfaces;
using SGHR.Application.Services;
using SGHR.IOC;
using SGHR.Persistence.Context;
using SGHR.Persistence.Interfaces;
using SGHR.Persistence.Repositories;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SGHRContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



builder.Services.AddControllersWithViews();
builder.Services.RegisterServices();  // REGISTRA TODOS TUS SERVICIOS

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Piso}/{action=Index}/{id?}");


app.Run();

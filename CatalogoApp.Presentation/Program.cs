using CatalogoApp.Application.Services;
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Infrastructure.Repositories;



var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews();



// Ruta del JSON
var jsonPath = Path.Combine(
 builder.Environment.ContentRootPath,
 "data",
 "items.json"
);



// Registrar repositorio
builder.Services.AddSingleton<IItemRepository>(
 new JsonItemRepository(jsonPath)
);



// Registrar servicio
builder.Services.AddScoped<ItemService>();



var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseRouting();



app.UseAuthorization();



app.MapStaticAssets();



app.MapControllerRoute(
 name: "default",
 pattern: "{controller=Home}/{action=Index}/{id?}")
 .WithStaticAssets();



app.Run();
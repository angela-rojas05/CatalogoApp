using CatalogoApp.Application.Services;
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Sesión para login
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

// Rutas de los archivos JSON (en la carpeta Data del proyecto)
var dataPath = Path.Combine(builder.Environment.ContentRootPath, "Data");

// Repositorios
builder.Services.AddSingleton<IItemRepository>(
    new JsonItemRepository(Path.Combine(dataPath, "items.json")));
builder.Services.AddSingleton(
    new JsonUsuarioRepository(Path.Combine(dataPath, "usuarios.json")));
builder.Services.AddSingleton(
    new JsonResenaRepository(Path.Combine(dataPath, "resenas.json")));

// Servicios de aplicación
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
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

// Ruta por defecto -> Catalogo directo
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Catalogo}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
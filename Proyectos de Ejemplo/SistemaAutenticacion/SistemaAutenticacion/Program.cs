using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using SistemaAutenticacion.Data;
using SistemaAutenticacion.Data.Permisos;
using SistemaAutenticacion.Data.PermsosRol;
using SistemaAutenticacion.Data.Roles;
using SistemaAutenticacion.Data.Usuario;
using SistemaAutenticacion.Middleware;
using SistemaAutenticacion.Profiles;
using SistemaAutenticacion.Token;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
{
    //Paso opcional
    options.LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information).EnableSensitiveDataLogging();

    //Configurar el proveedor de base de datos
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!);
});

builder.Services.AddHttpContextAccessor();

//Registro de servicios
builder.Services.AddScoped<IPermisosRepository, PermisosRepository>();
builder.Services.AddScoped<IPermisosRolRepository, PermisosRolRepository>();
builder.Services.AddScoped<IRolUsuarioRepository, RolUsuarioRepository>();
builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();


// Add services to the container.
// Permitir acceso a los controladores y vistas solo a usuarios autenticados a excepcion de los metodos allowanonymus
builder.Services.AddControllersWithViews(opt =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();

    //opt representa una instancia en los controladores
    opt.Filters.Add(new AuthorizeFilter(policy));
});


//Inyectar servicio de mapper para transformar datos de objetos entre objetos
var mapperConfig = new MapperConfiguration(MapperConfig =>
{
    //Registrar los perfiles de mapeo
    MapperConfig.AddProfile(new PermisosProfile());
    MapperConfig.AddProfile(new RolUsuarioProfile());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//Registro de middleware
app.UseMiddleware<ManagerMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaAutenticacion.Data;
using SistemaAutenticacion.Data.Permisos;
using SistemaAutenticacion.Data.PermsosRol;
using SistemaAutenticacion.Data.Roles;
using SistemaAutenticacion.Data.Usuario;
using SistemaAutenticacion.Middleware;
using SistemaAutenticacion.Models;
using SistemaAutenticacion.Profiles;
using SistemaAutenticacion.Token;
using System.Text;

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

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper); //Singleton para maneter el mismo objeto durante toda la vida de la aplicacion


//Inyectar el servicio de token
var builderSecurity = builder.Services.AddIdentityCore<Usuarios>();
var identityBuilder = new IdentityBuilder(builderSecurity.UserType, builder.Services);

builder.Services.AddIdentity<Usuarios, CustomRolUsuario>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

//Agregar el esquema para la migracion y creacion de las tablas en sql server
identityBuilder.AddEntityFrameworkStores<AppDbContext>();

//Inyectar el objeto que utilizaremos para el login
identityBuilder.AddSignInManager<SignInManager<Usuarios>>();

//Inyectar un sistem clock para controlar la hora en la que se registran los usuarios 
builder.Services.AddSingleton<ISystemClock, SystemClock>();

//Inyectar el gernerador de tokens JWT
builder.Services.AddScoped<IJwtGenerador, JwtGenerador>();

//Inyectar el usuarioSesion
builder.Services.AddScoped<IUsuarioSesion, UsuarioSesion>();

//Inyectar el respositorio de usuario 
builder.Services.AddScoped<IUsuariosRepository, UsuariosRepository>();

//Setear sistema de seguridad para que cada vez que ingrese un usuarios por un token, se valide si el token es correcto
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("EstaEsUnaClaveSuperSeguraDe32Bytes!"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    //Parametros a evaluar desde mi backend
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = false, //Validar la clave de firma del token
        IssuerSigningKey = key, //Objeto de clave simetrica
        ValidateIssuer = false, //No validar el emisor del token
        ValidateAudience = false, //No validar el receptor del token
        ValidateLifetime = true, //Validar la fecha de expiracion del token
        ClockSkew = TimeSpan.Zero //Sin desface de tiempo
    };
});

//Configuracion de Cors para que el cliente pueda acceder a los recursos del servidor Create, Read, Update, Delete
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsApp", builder =>
    {
        //builder.WithOrigins("https://sistemseguridad.gt, http://localhost:3000")
        builder.WithOrigins("*") //Permitir requests de cualquier origen (Tambien se puede configurar la IP especifica)
        //.AllowAnyOrigin() //Permitir cualquier origen (puede ser un dominio especifico o localhost)
        .AllowAnyMethod() //Permitir cualquier metodo HTTP (GET, POST, PUT, DELETE, etc.)
        .AllowAnyHeader(); //Permitir cualquier cabecera HTTP
    });
});

//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.LoginPath = "/Login/Index";
//    options.AccessDeniedPath = "/Login/Index";
//});



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

//Habilitar CORS
app.UseCors("CorsApp");

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

//Migrar la base de datos al iniciar la aplicacion 
using (var ambiente = app.Services.CreateScope())
{
    var service = ambiente.ServiceProvider;

    //Probar si la base de datos existe, si no existe se crea
    try
    {
        var userManager = service.GetRequiredService<UserManager<Usuarios>>();
        var context = service.GetRequiredService<AppDbContext>();

        //llamar al context para iniciar la migracion
        await context.Database.MigrateAsync(); //Evento para crear las tablas en base a los archivos de migracion

        //Insertar los datos de prueba
        await LoadDatabase.InsertarData(context, userManager);
    }
    catch (Exception e)
    {
        var logging = service.GetRequiredService<ILogger<Program>>();
        logging.LogError(e, "Ocurrio un error durante el proceso de migracion");
    }

}

app.Run();

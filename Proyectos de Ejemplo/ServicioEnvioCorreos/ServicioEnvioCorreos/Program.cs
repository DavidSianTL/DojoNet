using ServicioEnvioCorreos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ServicioEnvioCorreos.DAO;
using ServicioEnvioCorreos.Models;
using Serilog;

Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddFile("C:\\EnvioCorreo\\Logs\\correo-worker-{Date}.log");
    })
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        string cadenaConexion = context.Configuration.GetConnectionString("CorreoDb");
        // Vincula la configuración de correo
        var configuracionCorreo = new CorreoConfiguracion();
        context.Configuration.GetSection("Correo").Bind(configuracionCorreo);

        services.AddSingleton(configuracionCorreo);
        services.AddSingleton(new DAOCorreo(cadenaConexion));
        services.AddHostedService<Worker>();
    })
    .Build()
    .Run();

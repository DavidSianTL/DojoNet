using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using ServicioGeneracionFacturas;
using ServicioGeneracionFacturas.DAO;


//Compatibilidad para manejo de la libreria PDFSharp
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddEventLog(); 
    })
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<DAOFactura>(
            sp => new DAOFactura(context.Configuration.GetConnectionString("FacturaDb")));
        services.AddHostedService<Worker>();
    })
    .Build()
    .Run();
using ServicioGeneracionFacturas.Helper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using ServicioGeneracionFacturas.Models;
using ServicioGeneracionFacturas.DAO;

namespace ServicioGeneracionFacturas
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly DAOFactura _dao;
        private readonly string _rutaFacturas;

        public Worker(ILogger<Worker> logger, DAOFactura dao, IConfiguration config)
        {
            _logger = logger;
            _dao = dao;
            _rutaFacturas = config["Factura:RutaFacturas"];
        }
        public async Task EjecutarUnaVez()
        {
            _logger.LogInformation("Ejecutando facturación una sola vez: {time}", DateTimeOffset.Now);
            try
            {
                var pendientes = _dao.ObtenerFacturasPendientes();
                foreach (var factura in pendientes)
                {
                    var pdf = FacturaHelper.GenerarFacturaPDF(factura, _rutaFacturas);
                    _dao.MarcarComoFacturada(factura.Id, pdf);
                    _logger.LogInformation("Factura {id} generada en {pdf}", factura.Id, pdf);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en ejecución de facturación");
            }

            await Task.CompletedTask;
        }





        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Iniciando ciclo de facturación: {time}", DateTimeOffset.Now);
                try
                {
                    var pendientes = _dao.ObtenerFacturasPendientes();
                    foreach (var factura in pendientes)
                    {
                        var pdf = FacturaHelper.GenerarFacturaPDF(factura, _rutaFacturas);
                        _dao.MarcarComoFacturada(factura.Id, pdf);
                        _logger.LogInformation("Factura {id} generada en {pdf}", factura.Id, pdf);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error en ciclo de facturación");
                }
               // await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
                
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}

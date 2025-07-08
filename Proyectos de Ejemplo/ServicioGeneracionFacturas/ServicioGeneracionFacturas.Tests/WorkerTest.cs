using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using ServicioGeneracionFacturas.DAO;
using ServicioGeneracionFacturas.Models;
using Xunit;
using Moq;


namespace ServicioGeneracionFacturas.Tests;


public class WorkerTest
{
    [Fact]
    public async Task EjecutarUnaVez_DeberiaProcesarFacturasPendientes()
    {
       
        var loggerMock = new Mock<ILogger<Worker>>();

       
        var inMemorySettings = new Dictionary<string, string>
            {
                {"Factura:RutaFacturas", "C:\\Facturas\\Temp"}
            };

        IConfiguration config = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

       //base de datos de pruebas
        var dao = new DAOFactura("Server=HOME_PF\\SQLEXPRESS;Database=EmpresaDB_Desa;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

       
        var worker = new Worker(loggerMock.Object, dao, config);

       
        await worker.EjecutarUnaVez();

       
        var pendientes = dao.ObtenerFacturasPendientes();
        Assert.DoesNotContain(pendientes, f => f.Id == 1 || f.Id == 3);
    }
}


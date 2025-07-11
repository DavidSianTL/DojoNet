using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using ServicioGeneracionFacturas.DAO;
using ServicioGeneracionFacturas.Models;
using Xunit;
using Moq;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.SqlClient;
using System.Text;

namespace ServicioGeneracionFacturas.Tests;


public class WorkerTest2
{
    private readonly string _rutaFacturas = "C:\\Facturas\\Test\\Temp";


    [Fact]
    public async Task Worker_NoProcesaFacturasYaFacturadas()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        // Configuración mock
        var loggerMock = new Mock<ILogger<Worker>>();
        var settings = new Dictionary<string, string>
            {
                {"Factura:RutaFacturas", "C:\\Facturas\\Test\\Temp"}
            };

        IConfiguration config = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        Directory.CreateDirectory("C:\\Facturas\\Test\\Temp");

        var dao = new DAOFactura("Server=HOME_PF\\SQLEXPRESS;Database=EmpresaDB_Desa;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

        // Aseguramos que la factura con ID 2 esté como "Facturada"
        dao.MarcarComoFacturada(1, @"C:\Facturas\Test\Temp\Factura_1.pdf");

        var worker = new Worker(loggerMock.Object, dao, config);

        // Ejecuta el Worker
        await worker.EjecutarUnaVez();

        // Esta factura no debería cambiar su ruta PDF
        var factura2AunFacturada = dao.ObtenerFacturasPendientes();
        Assert.DoesNotContain(factura2AunFacturada, f => f.Id == 1);
    }



    [Fact]
    public async Task Worker_DeberiaGenerarArchivoPDFFisico()
    {
        //PrepararFacturaDePrueba(100);
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        // Preparar entorno
        Directory.CreateDirectory(_rutaFacturas);

        var loggerMock = new Mock<ILogger<Worker>>();
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                    {"Factura:RutaFacturas", _rutaFacturas}
            })
            .Build();

        var dao = new DAOFactura("Server=HOME_PF\\SQLEXPRESS;Database=EmpresaDB_Desa;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=true");

        //Insertar una factura de prueba directamente
        var idFactura = 100;
        //dao.MarcarComoFacturada(idFactura, null); // En caso de que ya exista, limpiarla
        dao.MarcarComoFacturada(idFactura, "");   // Borra ruta si quedó algo
        using (var conn = new SqlConnection("Server=HOME_PF\\SQLEXPRESS;Database=EmpresaDB_Desa;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=true"))
        {
            conn.Open();
            using var cmd = new SqlCommand($@"
                    DELETE FROM DetalleFactura WHERE IdFactura = {idFactura};
                    DELETE FROM FacturasPendientes WHERE Id = {idFactura};

                    INSERT INTO FacturasPendientes (Id, Cliente, Fecha, Estado, RutaPDF)
                    VALUES ({idFactura}, 'Test Cliente', GETDATE(), 'Pendiente', NULL);

                    INSERT INTO DetalleFactura (IdFactura, Producto, Cantidad, PrecioUnitario)
                    VALUES ({idFactura}, 'Producto Test', 2, 25.00);
                ", conn);
            cmd.ExecuteNonQuery();
        }

        // Ejecutar el worker
        var worker = new Worker(loggerMock.Object, dao, config);
        await worker.EjecutarUnaVez();

        // Verificar si el archivo existe
        var rutaEsperada = Path.Combine(_rutaFacturas, $"Factura_{idFactura}.pdf");
        Assert.True(File.Exists(rutaEsperada), $"No se encontró el PDF: {rutaEsperada}");

        // Limpiar el archivo generado
       // File.Delete(rutaEsperada);
    }

    private void PrepararFacturaDePrueba(int idFactura)
    {
        using (var conn = new SqlConnection("Server=HOME_PF\\SQLEXPRESS;Database=EmpresaDB_Desa;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=true"))
        {
            conn.Open();
            using var cmd = new SqlCommand($@"
            DELETE FROM DetalleFactura WHERE IdFactura = {idFactura};
            DELETE FROM FacturasPendientes WHERE Id = {idFactura};

            INSERT INTO FacturasPendientes (Id, Cliente, Fecha, Estado, RutaPDF)
            VALUES ({idFactura}, 'Test Cliente', GETDATE(), 'Pendiente', NULL);

            INSERT INTO DetalleFactura (IdFactura, Producto, Cantidad, PrecioUnitario)
            VALUES ({idFactura}, 'Producto Test', 2, 25.00);
        ", conn);
            cmd.ExecuteNonQuery();
        }
    }
}

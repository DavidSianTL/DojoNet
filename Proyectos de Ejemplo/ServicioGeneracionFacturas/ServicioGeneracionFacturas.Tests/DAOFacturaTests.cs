
using ServicioGeneracionFacturas.DAO;
using ServicioGeneracionFacturas.Models;
using Xunit;

namespace ServicioGeneracionFacturas.Tests;


public class DAOFacturaTests
{
    private readonly string _cadenaConexion = "Server=HOME_PF\\SQLEXPRESS;Database=EmpresaDB_Desa;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=true";

    [Fact]
    public void ObtenerFacturasPendientes_DeberiaDevolverFacturasConDetalles()
    {
        var dao = new DAOFactura(_cadenaConexion);

        var facturas = dao.ObtenerFacturasPendientes();

        Assert.NotNull(facturas);
        Assert.NotEmpty(facturas);
        Assert.All(facturas, f =>
        {
            Assert.True(f.Id > 0);
            Assert.False(string.IsNullOrWhiteSpace(f.Cliente));
            Assert.NotEmpty(f.Detalles);
        });
    }

    [Fact]
    public void MarcarComoFacturada_DeberiaActualizarEstado()
    {
        var dao = new DAOFactura(_cadenaConexion);
        var idFactura = 1;
        var rutaPDF = @"C:\Facturas\Test\Temp\Factura_1.pdf";

        dao.MarcarComoFacturada(idFactura, rutaPDF);

        // Revisión manual (idealmente leer desde DB para confirmar)
        var actualizadas = dao.ObtenerFacturasPendientes();
        Assert.DoesNotContain(actualizadas, f => f.Id == idFactura);
    }
}

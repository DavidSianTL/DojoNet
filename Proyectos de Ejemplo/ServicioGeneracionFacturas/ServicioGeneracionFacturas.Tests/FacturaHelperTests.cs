using ServicioGeneracionFacturas.Models;
using ServicioGeneracionFacturas.DAO;
using ServicioGeneracionFacturas.Helper;
using Xunit;


namespace ServicioGeneracionFacturas.Tests
{
    public class FacturaHelperTests
    {
        [Fact]
        public void GenerarFacturaPDF_DeberiaCrearArchivoPDF()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);


            var factura = new Factura
            {
                Id = 1,
                Cliente = "Prisila Flores",
                Fecha = DateTime.Today,
                Detalles = new List<DetalleFactura>
                { new DetalleFactura {Producto = "Tortrix Barbacoa", Cantidad = 5, PrecioUnitario = 2.00m },
                  new DetalleFactura {Producto = "Rosa de Jamaica", Cantidad = 5, PrecioUnitario = 3.00m }
              
                }
            };

            string rutaSalida = Path.Combine(Path.GetTempPath(), "C:\\Facturas\\Test\\");
            Directory.CreateDirectory(rutaSalida);

            string rutaArchivo = FacturaHelper.GenerarFacturaPDF(factura, rutaSalida);

            Assert.True(File.Exists(rutaArchivo), "El Archivo PDF no fue creado.");
        }
    }
}
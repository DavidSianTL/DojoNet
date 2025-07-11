using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using ServicioGeneracionFacturas.Models;

namespace ServicioGeneracionFacturas.Helper
{
    public static class FacturaHelper
    {
        public static string GenerarFacturaPDF(Factura factura, string rutaSalida)
        {
            string nombreArchivo = Path.Combine(rutaSalida, $"Factura_{factura.Id}.pdf");

            PdfDocument document = new PdfDocument();
            document.Info.Title = $"Factura {factura.Id}";
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Verdana", 12, XFontStyle.Regular);

            double y = 40;
            gfx.DrawString($"Factura N° {factura.Id}", font, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft);
            y += 30;
            gfx.DrawString($"Cliente: {factura.Cliente}", font, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft);
            y += 30;
            gfx.DrawString($"Fecha: {factura.Fecha:yyyy-MM-dd}", font, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft);
            y += 40;

            decimal total = 0;

            foreach (var item in factura.Detalles)
            {
                decimal subtotal = item.Cantidad * item.PrecioUnitario;
                gfx.DrawString($"{item.Producto} - {item.Cantidad} x {item.PrecioUnitario:C} = {subtotal:C}", font, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft);
                y += 25;
                total += subtotal;
            }

            y += 30;
            gfx.DrawString($"Total: {total:C}", new XFont("Verdana", 14, XFontStyle.Bold), XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft);

            document.Save(nombreArchivo);
            return nombreArchivo;
        }

    }
}

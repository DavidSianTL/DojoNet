using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using FastReport;
using FastReport.Export.PdfSimple;
using System.IO;
using wbSistemaSeguridadMVC.Services;
using wbSistemaSeguridadMVC.Data;

namespace wbSistemaSeguridadMVC.Controllers
{
    public class ReporteUController : Controller
    {

        private string connectionString = "Server=HOME_PF\\SQLEXPRESS;Database=SistemaSeguridad;Integrated Security=True;TrustServerCertificate=True;";
        private readonly daoUsuario _dbU;
    
        public ReporteUController(IConfiguration configuration)
        {
           _dbU = new daoUsuario(connectionString);
      
        }

        public async Task<IActionResult> UsuariosActivos()
        {
            try
            {
                DataTable dt = new DataTable();

                dt =  await _dbU.ObtenerUsuariosDataTable();

                using Report report = new Report();

                FastReport.Utils.RegisteredObjects.AddConnection(typeof(FastReport.Data.MsSqlDataConnection));
               
                report.Load("wwwroot/Reportes/ListadoUsuarios.frx");
                report.RegisterData(dt, "ListadoUsuarios");

                report.Prepare();

                using MemoryStream ms = new MemoryStream();
                PDFSimpleExport pdfExport = new PDFSimpleExport();
                report.Export(pdfExport, ms);
                ms.Position = 0;

                return File(ms.ToArray(), "application/pdf", "ListadoUsuarios.pdf");
            }
            catch (Exception ex)
            {
                return Content("Error al obtener los Usuarios: " + ex.Message);

            }

        }
        
    }
}

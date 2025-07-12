using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Services.Interfaces;
using OfficeOpenXml;
using System.Text;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class BitacoraController : Controller
    {
        private readonly daoBitacoraWSAsync _daoBitacoraWS;
        private readonly daoLogWSAsync _daoLog;
        private readonly daoUsuariosRolWSAsync _daoRolUsuario;
        private readonly ILoggingService _loggingService;

        public BitacoraController(
            daoBitacoraWSAsync daoBitacoraWS,
            daoLogWSAsync daoLog,
            daoUsuariosRolWSAsync daoRolUsuario,
            ILoggingService loggingService)
        {
            _daoBitacoraWS = daoBitacoraWS;
            _daoLog = daoLog;
            _daoRolUsuario = daoRolUsuario;
            _loggingService = loggingService;
        }

        private async Task RegistrarError(string accion, Exception ex)
        {
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            await _loggingService.RegistrarLogAsync(new LogViewModel
            {
                Accion = $"Error {accion}",
                Descripcion = $"Error al {accion} por {usuario}: {ex.Message}",
                Estado = false
            });
        }

        private async Task RegistrarBitacora(string accion, string descripcion)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

            await _daoBitacoraWS.InsertarBitacoraAsync(new BitacoraViewModel
            {
                Accion = accion,
                Descripcion = $"{descripcion} | Usuario: {usuario}",
                FK_IdUsuario = idUsuario,
                FK_IdSistema = idSistema,
                FechaEntrada = DateTime.Now
            });
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor", "Visualizador")]
        public async Task<IActionResult> Index(bool mostrarTodos = false)
        {
            try
            {
                var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var rolesUsuario = await _daoRolUsuario.ObtenerUsuariosRolPorIdUsuarioAsync(idUsuario);

                if (rolesUsuario == null)
                {
                    ViewBag.ErrorMessage = "Usuario no tiene el rol correcto asignado o no está activo.";
                    return View(new List<BitacoraViewModel>());
                }

                // Obtener todas las bitácoras
                var todasLasBitacoras = await _daoBitacoraWS.ObtenerBitacorasAsync();
                if (todasLasBitacoras == null)
                {
                    ViewBag.ErrorMessage = "No se pudieron cargar los registros de bitácora.";
                    return View(new List<BitacoraViewModel>());
                }

                List<BitacoraViewModel> bitacorasAMostrar;
                if (mostrarTodos)
                {
                    // Mostrar todos los registros
                    bitacorasAMostrar = todasLasBitacoras
                        .OrderByDescending(b => b.FechaEntrada)
                        .ToList();
                    ViewBag.MostrandoTodos = true;
                }
                else
                {
                    // Filtrar solo los registros del día actual del servidor
                    var fechaHoyServidor = DateTime.Now.Date;
                    bitacorasAMostrar = todasLasBitacoras
                        .Where(b => b.FechaEntrada.Date == fechaHoyServidor)
                        .OrderByDescending(b => b.FechaEntrada)
                        .ToList();
                    ViewBag.MostrandoTodos = false;
                }

                // Pasar información adicional a la vista
                ViewBag.FechaServidor = DateTime.Now.ToString("dd/MM/yyyy");
                ViewBag.TotalRegistrosDisponibles = todasLasBitacoras.Count;

                await RegistrarBitacora("Consulta Bitácora",
                    mostrarTodos ? "El usuario ha consultado todos los registros de bitácora"
                                 : "El usuario ha consultado los registros de bitácora del día actual");

                return View(bitacorasAMostrar);
            }
            catch (Exception e)
            {
                await RegistrarError("consultar bitácora", e);
                ViewBag.ErrorMessage = "Ocurrió un error al cargar los registros de bitácora.";
                return View(new List<BitacoraViewModel>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRole("SuperAdministrador")]
        public async Task<IActionResult> GuardarBitacora(BitacoraViewModel bitacora)
        {
            try
            {
                var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

                if (string.IsNullOrWhiteSpace(bitacora.Accion) ||
                    string.IsNullOrWhiteSpace(bitacora.Descripcion) ||
                    idUsuario <= 0 ||
                    idSistema <= 0)
                {
                    TempData["ErrorMessage"] = "Datos inválidos o sesión expirada.";
                    return RedirectToAction("Index");
                }

                bitacora.FK_IdUsuario = idUsuario;
                bitacora.FK_IdSistema = idSistema;
                bitacora.FechaEntrada = bitacora.FechaEntrada == default ? DateTime.Now : bitacora.FechaEntrada;

                await _daoBitacoraWS.InsertarBitacoraAsync(bitacora);

                await RegistrarBitacora("Insertar Bitácora Manual",
                    $"Se ha insertado manualmente una nueva bitácora con acción: {bitacora.Accion}");

                TempData["SuccessMessage"] = "Registro de bitácora guardado exitosamente.";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                await RegistrarError("guardar bitácora manual", e);
                TempData["ErrorMessage"] = $"Ocurrió un error al guardar el registro: {e.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor", "Visualizador")]
        public async Task<IActionResult> ObtenerBitacoras(int? idUsuario = null, string accion = null,
            string fechaDesde = null, string fechaHasta = null)
        {
            try
            {
                var registros = await ObtenerRegistrosFiltrados(idUsuario, accion, fechaDesde, fechaHasta);

                return Json(new
                {
                    success = true,
                    data = registros.Select(r => new {
                        IdBitacora = r.IdBitacora,
                        FechaEntrada = r.FechaEntrada,
                        Accion = r.Accion,
                        Descripcion = r.Descripcion,
                        FK_IdUsuario = r.FK_IdUsuario,
                        FK_IdSistema = r.FK_IdSistema
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador")]
        public async Task<IActionResult> ExportarExcel(int? idUsuario = null, string accion = null,
            string fechaDesde = null, string fechaHasta = null, string formato = "xlsx")
        {
            try
            {
                // Obtener datos filtrados
                var registros = await ObtenerRegistrosFiltrados(idUsuario, accion, fechaDesde, fechaHasta);

                if (!registros.Any())
                {
                    TempData["ErrorMessage"] = "No hay registros para exportar.";
                    return RedirectToAction("Index");
                }

                // Generar archivo Excel real (.xlsx)
                return await GenerarArchivoExcelXLSX(registros);
            }
            catch (Exception ex)
            {
                await RegistrarError("exportar bitácora a Excel", ex);
                TempData["ErrorMessage"] = $"Error al exportar: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador")]
        public async Task<IActionResult> ExportarPDF(int? idUsuario = null, string accion = null,
            string fechaDesde = null, string fechaHasta = null)
        {
            try
            {
                var registros = await ObtenerRegistrosFiltrados(idUsuario, accion, fechaDesde, fechaHasta);

                if (!registros.Any())
                {
                    TempData["ErrorMessage"] = "No hay registros para exportar.";
                    return RedirectToAction("Index");
                }

                // Crear HTML para PDF
                var html = GenerarHTMLParaPDF(registros, idUsuario, accion, fechaDesde, fechaHasta);
                var bytes = Encoding.UTF8.GetBytes(html);
                var fileName = $"Bitacora_{DateTime.Now:yyyyMMdd_HHmmss}.html";

                await RegistrarBitacora("Exportar Bitácora PDF",
                    $"El usuario ha exportado {registros.Count} registros de bitácora a PDF");

                return File(bytes, "text/html", fileName);
            }
            catch (Exception ex)
            {
                await RegistrarError("exportar bitácora a PDF", ex);
                TempData["ErrorMessage"] = "Error al exportar los registros de bitácora a PDF.";
                return RedirectToAction("Index");
            }
        }

        private async Task<IActionResult> GenerarArchivoExcelXLSX(List<BitacoraViewModel> registros)
        {
            // Configurar EPPlus para uso no comercial
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Bitácora");

                // Configurar encabezados
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Fecha";
                worksheet.Cells[1, 3].Value = "Hora";
                worksheet.Cells[1, 4].Value = "Acción";
                worksheet.Cells[1, 5].Value = "Descripción";
                worksheet.Cells[1, 6].Value = "ID Usuario";
                worksheet.Cells[1, 7].Value = "ID Sistema";

                // Estilo para encabezados
                using (var range = worksheet.Cells[1, 1, 1, 7])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                    range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                }

                // Llenar datos
                int row = 2;
                foreach (var registro in registros.OrderByDescending(r => r.FechaEntrada))
                {
                    worksheet.Cells[row, 1].Value = registro.IdBitacora;
                    worksheet.Cells[row, 2].Value = registro.FechaEntrada.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 3].Value = registro.FechaEntrada.ToString("HH:mm:ss");
                    worksheet.Cells[row, 4].Value = registro.Accion;
                    worksheet.Cells[row, 5].Value = registro.Descripcion;
                    worksheet.Cells[row, 6].Value = registro.FK_IdUsuario;
                    worksheet.Cells[row, 7].Value = registro.FK_IdSistema;

                    // Aplicar bordes a las celdas
                    using (var range = worksheet.Cells[row, 1, row, 7])
                    {
                        range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    }

                    row++;
                }

                // Ajustar ancho de columnas automáticamente
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                // Configurar ancho mínimo para columnas específicas
                worksheet.Column(1).Width = Math.Max(worksheet.Column(1).Width, 8);  // ID
                worksheet.Column(2).Width = Math.Max(worksheet.Column(2).Width, 12); // Fecha
                worksheet.Column(3).Width = Math.Max(worksheet.Column(3).Width, 10); // Hora
                worksheet.Column(4).Width = Math.Max(worksheet.Column(4).Width, 20); // Acción
                worksheet.Column(5).Width = Math.Max(worksheet.Column(5).Width, 40); // Descripción
                worksheet.Column(6).Width = Math.Max(worksheet.Column(6).Width, 12); // ID Usuario
                worksheet.Column(7).Width = Math.Max(worksheet.Column(7).Width, 12); // ID Sistema

                // Aplicar filtros automáticos
                worksheet.Cells[1, 1, row - 1, 7].AutoFilter = true;

                // Congelar la primera fila
                worksheet.View.FreezePanes(2, 1);

                // Generar nombre de archivo
                var fechaActual = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                var nombreArchivo = $"Bitacora_{fechaActual}.xlsx";

                // Convertir a bytes
                var fileBytes = package.GetAsByteArray();

                // Registrar la exportación
                await RegistrarBitacora("Exportar Bitácora Excel",
                    $"El usuario ha exportado {registros.Count} registros de bitácora a Excel (.xlsx)");

                // Retornar archivo
                return File(fileBytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    nombreArchivo);
            }
        }

        private async Task<List<BitacoraViewModel>> ObtenerRegistrosFiltrados(int? idUsuario, string accion,
            string fechaDesde, string fechaHasta)
        {
            try
            {
                var bitacoras = await _daoBitacoraWS.ObtenerBitacorasAsync();

                if (bitacoras == null)
                {
                    return new List<BitacoraViewModel>();
                }

                // Aplicar filtros
                if (idUsuario.HasValue)
                    bitacoras = bitacoras.Where(b => b.FK_IdUsuario == idUsuario.Value).ToList();

                if (!string.IsNullOrEmpty(accion))
                    bitacoras = bitacoras.Where(b => b.Accion.Contains(accion, StringComparison.OrdinalIgnoreCase)).ToList();

                if (!string.IsNullOrEmpty(fechaDesde) && DateTime.TryParse(fechaDesde, out var fechaDesdeDate))
                    bitacoras = bitacoras.Where(b => b.FechaEntrada.Date >= fechaDesdeDate.Date).ToList();

                if (!string.IsNullOrEmpty(fechaHasta) && DateTime.TryParse(fechaHasta, out var fechaHastaDate))
                    bitacoras = bitacoras.Where(b => b.FechaEntrada.Date <= fechaHastaDate.Date).ToList();

                return bitacoras.OrderByDescending(b => b.FechaEntrada).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener registros filtrados: {ex.Message}");
            }
        }

        private string GenerarHTMLParaPDF(List<BitacoraViewModel> bitacoras, int? idUsuario, string accion,
            string fechaDesde, string fechaHasta)
        {
            var html = new StringBuilder();
            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html>");
            html.AppendLine("<head>");
            html.AppendLine("<meta charset='utf-8'>");
            html.AppendLine("<title>Bitácora del Sistema</title>");
            html.AppendLine("<style>");
            html.AppendLine("body { font-family: Arial, sans-serif; margin: 20px; }");
            html.AppendLine("h1 { color: #333; text-align: center; }");
            html.AppendLine("table { width: 100%; border-collapse: collapse; margin-top: 20px; }");
            html.AppendLine("th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }");
            html.AppendLine("th { background-color: #f2f2f2; font-weight: bold; }");
            html.AppendLine("tr:nth-child(even) { background-color: #f9f9f9; }");
            html.AppendLine(".info { margin-bottom: 20px; padding: 10px; background-color: #e9ecef; border-radius: 5px; }");
            html.AppendLine("</style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            html.AppendLine("<h1>Bitácora del Sistema</h1>");

            // Información de filtros aplicados
            html.AppendLine("<div class='info'>");
            html.AppendLine($"<strong>Fecha de generación:</strong> {DateTime.Now:dd/MM/yyyy HH:mm:ss}<br>");
            html.AppendLine($"<strong>Total de registros:</strong> {bitacoras.Count}<br>");
            if (idUsuario.HasValue)
                html.AppendLine($"<strong>Filtro por Usuario ID:</strong> {idUsuario.Value}<br>");
            if (!string.IsNullOrEmpty(accion))
                html.AppendLine($"<strong>Filtro por Acción:</strong> {accion}<br>");
            if (!string.IsNullOrEmpty(fechaDesde))
                html.AppendLine($"<strong>Fecha desde:</strong> {fechaDesde}<br>");
            if (!string.IsNullOrEmpty(fechaHasta))
                html.AppendLine($"<strong>Fecha hasta:</strong> {fechaHasta}<br>");
            html.AppendLine("</div>");

            // Tabla de datos
            html.AppendLine("<table>");
            html.AppendLine("<thead>");
            html.AppendLine("<tr>");
            html.AppendLine("<th>ID</th>");
            html.AppendLine("<th>Fecha</th>");
            html.AppendLine("<th>Hora</th>");
            html.AppendLine("<th>Acción</th>");
            html.AppendLine("<th>Descripción</th>");
            html.AppendLine("<th>Usuario ID</th>");
            html.AppendLine("<th>Sistema ID</th>");
            html.AppendLine("</tr>");
            html.AppendLine("</thead>");
            html.AppendLine("<tbody>");

            foreach (var registro in bitacoras)
            {
                html.AppendLine("<tr>");
                html.AppendLine($"<td>{registro.IdBitacora}</td>");
                html.AppendLine($"<td>{registro.FechaEntrada:dd/MM/yyyy}</td>");
                html.AppendLine($"<td>{registro.FechaEntrada:HH:mm:ss}</td>");
                html.AppendLine($"<td>{System.Web.HttpUtility.HtmlEncode(registro.Accion)}</td>");
                html.AppendLine($"<td>{System.Web.HttpUtility.HtmlEncode(registro.Descripcion)}</td>");
                html.AppendLine($"<td>{registro.FK_IdUsuario}</td>");
                html.AppendLine($"<td>{registro.FK_IdSistema}</td>");
                html.AppendLine("</tr>");
            }

            html.AppendLine("</tbody>");
            html.AppendLine("</table>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");

            return html.ToString();
        }
    }
}

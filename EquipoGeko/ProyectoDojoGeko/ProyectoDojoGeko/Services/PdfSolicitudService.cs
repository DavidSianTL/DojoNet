using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO.Compression;
using System.Text;


/*===============================================
==   Service: PdfSolicitudService               = 
=================================================*/

/***Generación de PDF: Usa wkhtmltopdf con plantilla HTML que replica exactamente tu formato
**Compresión Brotli: Reduce significativamente el tamaño de almacenamiento
**Almacenamiento en DB: PDFs comprimidos se guardan en tabla `SolicitudPDF`
**Control de Descarga: Permite descarga solo hasta que se apruebe la solicitud
**Gestión Automática: Se crea el PDF al crear la solicitud y se restringe al aprobar*/



namespace ProyectoDojoGeko.Services
{
    public interface IPdfSolicitudService
    {
        Task<byte[]> GenerarPDFSolicitudAsync(int idSolicitud);
        Task<bool> GuardarPDFEnBaseDatosAsync(int idSolicitud, byte[] pdfBytes);
        Task<(byte[] contenido, string nombreArchivo)?> ObtenerPDFSolicitudAsync(int idSolicitud);
        Task RestringirDescargaPDFAsync(int idSolicitud);
    }

    public class PdfSolicitudService : IPdfSolicitudService
    {
        private readonly string _connectionString;
        private readonly string _wkhtmltopdfPath;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<PdfSolicitudService> _logger;

        public PdfSolicitudService(
            IConfiguration configuration,
            IWebHostEnvironment environment,
            ILogger<PdfSolicitudService> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _wkhtmltopdfPath = Path.Combine(environment.ContentRootPath, "tools", "wkhtmltopdf.exe");
            _environment = environment;
            _logger = logger;
        }

        public async Task<byte[]> GenerarPDFSolicitudAsync(int idSolicitud)
        {
            try
            {
                // 1. Obtener datos de la solicitud
                var datosSolicitud = await ObtenerDatosSolicitudAsync(idSolicitud);
                if (datosSolicitud == null)
                    throw new Exception($"No se encontró la solicitud con ID: {idSolicitud}");

                // 2. Generar HTML desde la plantilla
                var htmlContent = GenerarHTMLSolicitud(datosSolicitud);

                // 3. Crear archivo temporal HTML
                var tempHtmlFile = Path.GetTempFileName() + ".html";
                var tempPdfFile = Path.GetTempFileName() + ".pdf";

                try
                {
                    await File.WriteAllTextAsync(tempHtmlFile, htmlContent, Encoding.UTF8);

                    // 4. Ejecutar wkhtmltopdf
                    var processInfo = new ProcessStartInfo
                    {
                        FileName = _wkhtmltopdfPath,
                        Arguments = $"--page-size A4 --margin-top 20mm --margin-bottom 20mm --margin-left 15mm --margin-right 15mm --encoding UTF-8 \"{tempHtmlFile}\" \"{tempPdfFile}\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };

                    using (var process = Process.Start(processInfo))
                    {
                        await process.WaitForExitAsync();

                        if (process.ExitCode != 0)
                        {
                            var error = await process.StandardError.ReadToEndAsync();
                            throw new Exception($"Error generando PDF: {error}");
                        }
                    }

                    // 5. Leer el PDF generado
                    if (!File.Exists(tempPdfFile))
                        throw new Exception("El archivo PDF no fue generado correctamente");

                    return await File.ReadAllBytesAsync(tempPdfFile);
                }
                finally
                {
                    // Limpiar archivos temporales
                    if (File.Exists(tempHtmlFile)) File.Delete(tempHtmlFile);
                    if (File.Exists(tempPdfFile)) File.Delete(tempPdfFile);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando PDF para solicitud {IdSolicitud}", idSolicitud);
                throw;
            }
        }

        public async Task<bool> GuardarPDFEnBaseDatosAsync(int idSolicitud, byte[] pdfBytes)
        {
            try
            {
                // Comprimir con Brotli
                var compressedBytes = ComprimirConBrotli(pdfBytes);
                var nombreArchivo = $"Solicitud_Vacaciones_{idSolicitud}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("sp_InsertarSolicitudPDF", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FK_IdSolicitud", idSolicitud);
                command.Parameters.AddWithValue("@NombreArchivo", nombreArchivo);
                command.Parameters.AddWithValue("@ContenidoPDFComprimido", compressedBytes);
                command.Parameters.AddWithValue("@TamanoOriginal", pdfBytes.Length);
                command.Parameters.AddWithValue("@TamanoComprimido", compressedBytes.Length);

                await connection.OpenAsync();
                var result = await command.ExecuteScalarAsync();

                return result != null && Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando PDF en base de datos para solicitud {IdSolicitud}", idSolicitud);
                return false;
            }
        }

        public async Task<(byte[] contenido, string nombreArchivo)?> ObtenerPDFSolicitudAsync(int idSolicitud)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("sp_ObtenerSolicitudPDF", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FK_IdSolicitud", idSolicitud);

                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    var estadoSolicitud = reader.GetInt32("FK_IdEstadoSolicitud");
                    var estadoPdf = reader.GetInt32("FK_IdEstado");

                    // Verificar si la descarga está permitida
                    if (estadoSolicitud == 2 && estadoPdf == 4) // 2=Autorizada, 4=Restringido
                    {
                        return null; // Descarga restringida
                    }

                    var contenidoComprimido = (byte[])reader["ContenidoPDFComprimido"];
                    var nombreArchivo = reader.GetString("NombreArchivo");

                    // Descomprimir
                    var contenidoDescomprimido = DescomprimirBrotli(contenidoComprimido);

                    return (contenidoDescomprimido, nombreArchivo);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo PDF para solicitud {IdSolicitud}", idSolicitud);
                return null;
            }
        }

        public async Task RestringirDescargaPDFAsync(int idSolicitud)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                using var command = new SqlCommand("sp_RestringirDescargaPDF", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FK_IdSolicitud", idSolicitud);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error restringiendo descarga PDF para solicitud {IdSolicitud}", idSolicitud);
                throw;
            }
        }

        private async Task<DatosSolicitudPDF> ObtenerDatosSolicitudAsync(int idSolicitud)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(@"
                SELECT 
                    se.IdSolicitud,
                    se.NombresEmpleado,
                    se.DiasSolicitadosTotal,
                    se.FechaIngresoSolicitud,
                    se.Observaciones,
                    e.Puesto,
                    e.Departamento,
                    MIN(sd.FechaInicio) as FechaInicio,
                    MAX(sd.FechaFin) as FechaFin
                FROM SolicitudEncabezado se
                INNER JOIN Empleados e ON se.FK_IdEmpleado = e.IdEmpleado
                LEFT JOIN SolicitudDetalle sd ON se.IdSolicitud = sd.FK_IdSolicitud
                WHERE se.IdSolicitud = @IdSolicitud
                GROUP BY se.IdSolicitud, se.NombresEmpleado, se.DiasSolicitadosTotal, 
                         se.FechaIngresoSolicitud, se.Observaciones, e.Puesto, e.Departamento", connection);

            command.Parameters.AddWithValue("@IdSolicitud", idSolicitud);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new DatosSolicitudPDF
                {
                    IdSolicitud = reader.GetInt32("IdSolicitud"),
                    NombreEmpleado = reader.GetString("NombresEmpleado"),
                    Puesto = reader.IsDBNull("Puesto") ? "" : reader.GetString("Puesto"),
                    Departamento = reader.IsDBNull("Departamento") ? "" : reader.GetString("Departamento"),
                    DiasSolicitados = reader.GetDecimal("DiasSolicitadosTotal"),
                    FechaSolicitud = reader.GetDateTime("FechaIngresoSolicitud"),
                    FechaInicio = reader.IsDBNull("FechaInicio") ? null : reader.GetDateTime("FechaInicio"),
                    FechaFin = reader.IsDBNull("FechaFin") ? null : reader.GetDateTime("FechaFin"),
                    Observaciones = reader.IsDBNull("Observaciones") ? "" : reader.GetString("Observaciones")
                };
            }

            return null;
        }

        private string GenerarHTMLSolicitud(DatosSolicitudPDF datos)
        {
            var fechaActual = datos.FechaSolicitud.ToString("dd/MM/yyyy");
            var periodoCorrespondiente = datos.FechaInicio.HasValue && datos.FechaFin.HasValue
                ? $"{datos.FechaInicio.Value:dd/MM/yyyy} AL {datos.FechaFin.Value:dd/MM/yyyy}"
                : "Por definir";

            return $@"
<!DOCTYPE html>
<html lang='es'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Solicitud de Días de Disponibilidad</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            font-size: 12px;
            line-height: 1.4;
            margin: 0;
            padding: 20px;
        }}
        .header {{
            text-align: center;
            margin-bottom: 30px;
        }}
        .title {{
            font-size: 16px;
            font-weight: bold;
            margin-bottom: 20px;
        }}
        .fecha {{
            text-align: right;
            margin-bottom: 20px;
        }}
        .content {{
            margin-bottom: 30px;
            text-align: justify;
        }}
        .dias-periodo {{
            text-align: center;
            font-weight: bold;
            margin: 20px 0;
            font-size: 14px;
        }}
        .observaciones {{
            margin: 30px 0;
        }}
        .observaciones-label {{
            font-weight: bold;
            margin-bottom: 10px;
        }}
        .observaciones-content {{
            border: 1px solid #000;
            min-height: 80px;
            padding: 10px;
        }}
        .firmas {{
            margin-top: 60px;
            display: flex;
            justify-content: space-between;
        }}
        .firma {{
            text-align: center;
            width: 45%;
        }}
        .linea-firma {{
            border-bottom: 1px solid #000;
            margin-bottom: 5px;
            height: 40px;
        }}
        .underline {{
            border-bottom: 1px solid #000;
            display: inline-block;
            min-width: 200px;
            text-align: center;
        }}
    </style>
</head>
<body>
    <div class='header'>
        <div class='title'>SOLICITUD DE DIAS DE DISPONIBILIDAD. GDG</div>
    </div>

    <div class='fecha'>
        Fecha: {fechaActual}
    </div>

    <div class='content'>
        <p>Señores<br>
        Grupo Digital de Guatemala<br>
        Presente.</p>

        <p style='margin-top: 30px;'>
        Por este medio hago de su conocimiento que el (la) señor (ita) 
        <span class='underline'>{datos.NombreEmpleado}</span> que aporta su industria como: 
        <span class='underline'>{datos.Puesto}</span> en el Departamento de 
        <span class='underline'>{datos.Departamento}</span> tomará 
        <span class='underline'>{datos.DiasSolicitados}</span> días de disponibilidad, 
        correspondientes al periodo: <span class='underline'>{periodoCorrespondiente}</span>
        </p>
    </div>

    <div class='dias-periodo'>
        LOS DÍAS LOS TOMARÁ DEL {(datos.FechaInicio?.ToString("dd/MM/yyyy") ?? "___/___/___")} 
        AL {(datos.FechaFin?.ToString("dd/MM/yyyy") ?? "___/___/___")} DEL AÑO EN CURSO.
    </div>

    <div class='observaciones'>
        <div class='observaciones-label'>Observaciones:</div>
        <div class='observaciones-content'>
            {datos.Observaciones}
        </div>
    </div>

    <div class='firmas'>
        <div class='firma'>
            <div class='linea-firma'></div>
            <div>(f) Director o encargado</div>
        </div>
        <div class='firma'>
            <div class='linea-firma'></div>
            <div>(f) Socio Industrial</div>
        </div>
    </div>
</body>
</html>";
        }

        private byte[] ComprimirConBrotli(byte[] data)
        {
            using var output = new MemoryStream();
            using var brotliStream = new BrotliStream(output, CompressionLevel.Optimal);
            brotliStream.Write(data, 0, data.Length);
            brotliStream.Close();
            return output.ToArray();
        }

        private byte[] DescomprimirBrotli(byte[] compressedData)
        {
            using var input = new MemoryStream(compressedData);
            using var brotliStream = new BrotliStream(input, CompressionMode.Decompress);
            using var output = new MemoryStream();
            brotliStream.CopyTo(output);
            return output.ToArray();
        }
    }

    public class DatosSolicitudPDF
    {
        public int IdSolicitud { get; set; }
        public string NombreEmpleado { get; set; }
        public string Puesto { get; set; }
        public string Departamento { get; set; }
        public decimal DiasSolicitados { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Observaciones { get; set; }
    }
}

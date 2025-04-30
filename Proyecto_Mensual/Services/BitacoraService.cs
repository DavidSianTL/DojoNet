using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using AppFuturista.Models;

namespace AppFuturista.Services
{
    public class BitacoraService
    {
        private readonly string _rutaBitacora;
        private readonly string _rutaErrores;
        
        public BitacoraService(IHostEnvironment env, ArchivoJsonService archivoJsonService)
        {
            var configuracion = archivoJsonService.LeerConfiguracionAsync<ConfiguracionApp>("configuracion.json").Result;
            
            _rutaBitacora = Path.Combine(env.ContentRootPath, configuracion.RutaBitacora);
            _rutaErrores = Path.Combine(env.ContentRootPath, configuracion.RutaErrores);
            
            // Asegurar que los directorios existen
            Directory.CreateDirectory(Path.GetDirectoryName(_rutaBitacora));
            Directory.CreateDirectory(Path.GetDirectoryName(_rutaErrores));
        }
        
        public async Task RegistrarEventoAsync(string modulo, string mensaje)
        {
            var entrada = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {modulo} | {mensaje}";
            
            try
            {
                await File.AppendAllTextAsync(_rutaBitacora, entrada + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // Si no podemos escribir en la bitácora, intentamos registrar el error
                try
                {
                    await RegistrarErrorAsync("BitacoraService", $"Error al registrar evento: {ex.Message}");
                }
                catch
                {
                    // No podemos hacer nada más si ambos archivos fallan
                }
            }
        }
        
        public async Task RegistrarErrorAsync(string modulo, string mensaje, Exception ex = null)
        {
            var detalleError = ex != null ? $" | Excepción: {ex.GetType().Name} | {ex.Message} | {ex.StackTrace}" : "";
            var entrada = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | ERROR | {modulo} | {mensaje}{detalleError}";
            
            try
            {
                await File.AppendAllTextAsync(_rutaErrores, entrada + Environment.NewLine);
            }
            catch
            {
                // No podemos hacer nada más si el archivo de errores falla
            }
        }
        
        public async Task<string[]> ObtenerEventosAsync(int cantidadLineas = 100)
        {
            if (!File.Exists(_rutaBitacora))
            {
                return Array.Empty<string>();
            }
            
            var todasLasLineas = await File.ReadAllLinesAsync(_rutaBitacora);
            
            if (todasLasLineas.Length <= cantidadLineas)
            {
                return todasLasLineas;
            }
            
            var resultado = new string[cantidadLineas];
            Array.Copy(todasLasLineas, todasLasLineas.Length - cantidadLineas, resultado, 0, cantidadLineas);
            
            return resultado;
        }
        
        public async Task<string[]> ObtenerErroresAsync(int cantidadLineas = 100)
        {
            if (!File.Exists(_rutaErrores))
            {
                return Array.Empty<string>();
            }
            
            var todasLasLineas = await File.ReadAllLinesAsync(_rutaErrores);
            
            if (todasLasLineas.Length <= cantidadLineas)
            {
                return todasLasLineas;
            }
            
            var resultado = new string[cantidadLineas];
            Array.Copy(todasLasLineas, todasLasLineas.Length - cantidadLineas, resultado, 0, cantidadLineas);
            
            return resultado;
        }
    }
}

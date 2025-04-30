using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace AppFuturista.Services
{
    public class ArchivoJsonService
    {
        private readonly string _rutaBase;
        
        public ArchivoJsonService(IHostEnvironment env)
        {
            _rutaBase = Path.Combine(env.ContentRootPath, "Data");
            
            // Asegurar que el directorio existe
            if (!Directory.Exists(_rutaBase))
            {
                Directory.CreateDirectory(_rutaBase);
            }
        }
        
        public async Task<List<T>> LeerAsync<T>(string nombreArchivo)
        {
            var rutaArchivo = Path.Combine(_rutaBase, nombreArchivo);
            
            if (!File.Exists(rutaArchivo))
            {
                return new List<T>();
            }
            
            using var stream = File.OpenRead(rutaArchivo);
            return await JsonSerializer.DeserializeAsync<List<T>>(stream) ?? new List<T>();
        }
        
        public async Task EscribirAsync<T>(string nombreArchivo, List<T> datos)
        {
            var rutaArchivo = Path.Combine(_rutaBase, nombreArchivo);
            
            using var stream = File.Create(rutaArchivo);
            var opciones = new JsonSerializerOptions { WriteIndented = true };
            await JsonSerializer.SerializeAsync(stream, datos, opciones);
        }
        
        public async Task<T> LeerConfiguracionAsync<T>(string nombreArchivo) where T : new()
        {
            var rutaArchivo = Path.Combine(_rutaBase, nombreArchivo);
            
            if (!File.Exists(rutaArchivo))
            {
                var configuracionPredeterminada = new T();
                await EscribirConfiguracionAsync(nombreArchivo, configuracionPredeterminada);
                return configuracionPredeterminada;
            }
            
            using var stream = File.OpenRead(rutaArchivo);
            return await JsonSerializer.DeserializeAsync<T>(stream) ?? new T();
        }
        
        public async Task EscribirConfiguracionAsync<T>(string nombreArchivo, T datos)
        {
            var rutaArchivo = Path.Combine(_rutaBase, nombreArchivo);
            
            using var stream = File.Create(rutaArchivo);
            var opciones = new JsonSerializerOptions { WriteIndented = true };
            await JsonSerializer.SerializeAsync(stream, datos, opciones);
        }
    }
}

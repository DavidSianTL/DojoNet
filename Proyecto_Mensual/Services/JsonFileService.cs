using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace RazorCrudApp.Services
{
    public class JsonFileService
    {
        private readonly string _basePath;
        
        public JsonFileService(string contentRootPath)
        {
            _basePath = Path.Combine(contentRootPath, "Data");
            
            // Ensure the directory exists
            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }
        
        public async Task<List<T>> ReadAsync<T>(string fileName)
        {
            var filePath = Path.Combine(_basePath, fileName);
            
            if (!File.Exists(filePath))
            {
                return new List<T>();
            }
            
            using var stream = File.OpenRead(filePath);
            return await JsonSerializer.DeserializeAsync<List<T>>(stream) ?? new List<T>();
        }
        
        public async Task WriteAsync<T>(string fileName, List<T> data)
        {
            var filePath = Path.Combine(_basePath, fileName);
            
            using var stream = File.Create(filePath);
            var options = new JsonSerializerOptions { WriteIndented = true };
            await JsonSerializer.SerializeAsync(stream, data, options);
        }
    }
}

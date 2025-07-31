using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace ProyectoDojoGeko.Services
{
    public interface ICloudinaryService
    {
        // Interfaz para el servicio de Cloudinary que define el método para subir imágenes
        Task<string> UploadImageAsync(IFormFile file, string idPublic = null, string folder = null);
    }

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        // Constructor que recibe la configuración de Cloudinary desde el archivo appsettings.json
        public CloudinaryService(IConfiguration configuration)
        {
            var cloudName = configuration["Cloudinary:CloudName"];
            var apiKey = configuration["Cloudinary:ApiKey"];
            var apiSecret = configuration["Cloudinary:ApiSecret"];

            // Usamos "Account" para configurar Cloudinary con las credenciales
            var account = new Account(cloudName, apiKey, apiSecret);

            // Inicializamos Cloudinary con la cuenta configurada
            _cloudinary = new Cloudinary(account);
        }

        // Método para subir una imagen a Cloudinary
        public async Task<string> UploadImageAsync(IFormFile file, string idPublic = null, string folder = null)
        {
            // Validar el archivo
            if (file == null || file.Length == 0)
                throw new ArgumentException("No se proporcionó ningún archivo.");

            // Usamos el "OpdenReadStream" para leer el archivo
            using (var stream = file.OpenReadStream())
            {
                // Si no se proporciona un publicId, generamos uno basado en el nombre del archivo
                var uploadParams = new ImageUploadParams
                {
                    // Usamos "FileDescription" para manejar el archivo
                    File = new FileDescription(file.FileName, stream),
                    PublicId = idPublic,
                    Folder = folder,
                    Overwrite = true,
                    UseFilename = true
                };

                // Subimos la imagen a Cloudinary
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                // Verificamos si la subida fue exitosa
                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Retornamos la URL segura de la imagen subida
                    return uploadResult.SecureUrl.ToString();
                }
                else
                {
                    throw new Exception("Error al subir la imagen a Cloudinary: " + uploadResult.Error?.Message);
                }
            }
        }
    }
}


using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Services.Interfaces;
using ProyectoDojoGeko.Services;
using ProyectoDojoGeko.Dtos.Empleados.Requests;

namespace ProyectoDojoGeko.Controllers
{
    public class ConfiguracionController : Controller
    {

        // Instanciamos el daoEmpleado
        private readonly daoEmpleadoWSAsync _daoEmpleado;

        // Instanciamos el servicio de Cloudinary
        private readonly ICloudinaryService _cloudinaryService; 

        // Instanciamos el servicio de logging
        private readonly ILoggingService _loggingService;

        // Instanciamos el servicio de bitácora
        private readonly IBitacoraService _bitacoraService;

        // Constructor que recibe las dependencias
        public ConfiguracionController(daoEmpleadoWSAsync daoEmpleado, ICloudinaryService cloudinaryService, ILoggingService loggingService, IBitacoraService bitacoraService)
        {
            _daoEmpleado = daoEmpleado;
            _cloudinaryService = cloudinaryService;
            _loggingService = loggingService;
            _bitacoraService = bitacoraService;
        }

        // Método para registrar errores en el log
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

        // GET: Configuracion
        public async Task<IActionResult> Index()
        {
            return View(); 
        }

        // POST: Configuracion/CambiarFoto
        [HttpPost]
        public async Task<IActionResult> CambiarFoto(FotoEmpleadoRequest model)
        {
            // Verificamos que la foto no sea nula y tenga contenido
            try
            {
                // Verificamos que el modelo sea válido
                if (!ModelState.IsValid)
                {
                    // Debug: Log error de validación
                    await RegistrarError("Cambiar Foto", new Exception("Modelo no válido al cambiar la foto."));
                    return RedirectToAction("Index");
                }

                // Le pasamos el ID del empleado desde la sesión
                model.IdEmpleado = HttpContext.Session.GetInt32("IdEmpleado") ?? 0;

                // Subimos la imagen a Cloudinary
                var url = await _cloudinaryService.UploadImageAsync(
                    model.FotoPerfil,
                    // "Ticks" para generar un publicId único
                    //$"{model.IdEmpleado}_{DateTime.UtcNow.Ticks}",

                    // Usamos el ID del empleado como publicId en este caso
                    // ya que no vamos a almacenar varias imagenes
                    model.IdEmpleado.ToString(), 
                    "perfiles" // carpeta en Cloudinary
                );

                // Guarda la ruta en la base de datos
                await _daoEmpleado.GuardarRutaFotoPerfil(model.IdEmpleado, url);

                TempData["Mensaje"] = "Foto de perfil actualizada correctamente.";
                TempData["TipoMensaje"] = "success";
            }
            catch (Exception ex)
            {
                
                await RegistrarError("guardar foto de perfil", ex);

            }

            return RedirectToAction("Index");
        }

 
    }
}
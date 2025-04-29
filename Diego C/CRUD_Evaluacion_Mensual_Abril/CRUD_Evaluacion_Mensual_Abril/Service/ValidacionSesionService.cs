namespace CRUD_Evaluacion_Mensual_Abril.Service
{
    using Microsoft.AspNetCore.Http;
    // Interfaz para el servicio de validación de sesión
    // Esta interfaz define un método para validar la sesión del usuario
    // y se puede implementar en diferentes clases según las necesidades de la aplicación.

    public interface IServicioSesion
    {
        bool EsSesionValida();
    }

    public class ValidacionSesionService : IServicioSesion
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidacionSesionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool EsSesionValida()
        {
            var usuario = _httpContextAccessor.HttpContext.Session.GetString("usrNombre");
            return !string.IsNullOrEmpty(usuario);
        }
    }
}

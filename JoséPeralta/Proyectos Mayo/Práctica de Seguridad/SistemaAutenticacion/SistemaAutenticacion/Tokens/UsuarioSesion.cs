using System.Security.Claims;

namespace SistemaAutenticacion.Tokens
{
    // Interfaz para obtener diferentes datos de la sesión del usuario
    public interface IUsuarioSesion
    {
        string ObtenerUsuarioSesion();
    }

    public class UsuarioSesion : IUsuarioSesion
    {
        // Esta clase se encarga de obtener la información del usuario desde el contexto HTTP
        private readonly IHttpContextAccessor _httpContextAccessor;

        // Constructor que recibe IHttpContextAccessor para acceder al contexto HTTP
        public UsuarioSesion(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // Método para obtener el nombre de usuario de la sesión actual
        public string ObtenerUsuarioSesion()
        {
            // Intenta obtener el nombre de usuario del contexto HTTP
            // Usamos el FirstOrDefault para evitar excepciones si no se encuentra el claim
            // Dentro de este, le decimos que busque el claim de tipo NameIdentifier
            // y que devuelva su valor
            var username = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return string.Empty; 
            }

            return username; // Devuelve el nombre de usuario o el ID del usuario según tu implementación

        }



    }
}

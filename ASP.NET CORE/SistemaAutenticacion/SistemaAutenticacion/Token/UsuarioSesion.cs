using System.Security.Claims;

namespace SistemaAutenticacion.Token
{
    public interface IUsuarioSesion
    {
        string ObtenerUsuarioSesion();
    }
    public class UsuarioSesion : IUsuarioSesion
    {
        //Toma los datos del usuario que está autenticado por medio de los Http
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioSesion(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public string ObtenerUsuarioSesion()
        {
            //Accede al contexto al http actual, "!" verifica que no sea null, User claims accede al usuario y a sus claims o datos
            //FirsOfDefault busca el dato del usuario, Value extrae el valor del claim
            var Username = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            
            //Devuelve el usuario en tipo string
            //"!" indica que el usuario definitivamente no será null el username
            return Username!;
        }
    }
}

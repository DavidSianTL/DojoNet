using System.Security.Claims;

namespace SistemaAutenticacionAPI.Token
{
    public interface IUsuarioSesion
    {
        string ObtenerUsuarioSesion();
    }

    public class UsuarioSesion: IUsuarioSesion
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioSesion(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string ObtenerUsuarioSesion()
        {
            var UserName = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            return UserName!;
        }

    }
}

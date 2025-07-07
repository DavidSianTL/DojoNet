using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Services
{
    public interface IBitacoraService
    {
        public Task RegistrarBitacoraAsync(string accion, string descripcion);

    }

    public class BitacoraService :IBitacoraService
    {
        private readonly daoBitacoraWSAsync _daoBitacoraWS;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BitacoraService(daoBitacoraWSAsync daoBitacoraWS, IHttpContextAccessor httpContextAccessor)
        {
            _daoBitacoraWS = daoBitacoraWS;
            _httpContextAccessor = httpContextAccessor;
        }

        // Método para registrar acciones en la bitácora
        public async Task RegistrarBitacoraAsync(string accion, string descripcion)
        {
            var context = _httpContextAccessor.HttpContext;
            // Verificar que el HttpContext no es nulo
            if (context is null) throw new InvalidOperationException("No hay ninguna sesión activa");
          
            var session = context.Session;

            // Obtiene el ID de usuario, nombre de usuario y ID del sistema desde la sesión
            var idUsuario = session.GetInt32("IdUsuario") ?? 0;
            var usuario = session.GetString("Usuario") ?? "Desconocido";
            var idSistema = session.GetInt32("IdSistema") ?? 0;
            // Inserta una nueva entrada en la bitácora
            await _daoBitacoraWS.InsertarBitacoraAsync(new BitacoraViewModel
            {
                Accion = accion,
                Descripcion = $"{descripcion} | Usuario: {usuario}",
                FK_IdUsuario = idUsuario,
                FK_IdSistema = idSistema
            });
        }

    }
}

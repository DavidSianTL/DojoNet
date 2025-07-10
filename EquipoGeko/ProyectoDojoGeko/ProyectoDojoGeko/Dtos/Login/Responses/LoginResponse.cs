using System;
using ProyectoDojoGeko.Dtos.Usuarios.Responses;

namespace ProyectoDojoGeko.Dtos.Login.Responses
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime ExpiraEn { get; set; }
        public UsuarioResponse Usuario { get; set; } = null!;
    }
}

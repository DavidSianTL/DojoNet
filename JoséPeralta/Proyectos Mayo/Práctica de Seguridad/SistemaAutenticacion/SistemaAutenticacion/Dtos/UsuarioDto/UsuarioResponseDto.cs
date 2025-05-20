namespace SistemaAutenticacion.Dtos.UsuarioDto
{
    // Manejo de la respuesta al iniciar sesion
    // Se utiliza para enviar el token y los datos del usuario al cliente
    public class UsuarioResponseDto
    {
        public string? Token { get; set; }
        public string? Id { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }

    }
}

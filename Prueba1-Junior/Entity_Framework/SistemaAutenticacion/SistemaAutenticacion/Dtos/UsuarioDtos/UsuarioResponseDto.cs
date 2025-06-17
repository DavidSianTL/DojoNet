namespace SistemaAutenticacion.Dtos.UsuarioDtos
{
    /// <summary>
    /// Manejo de datos devueltos desde el backend
    /// </summary>
    public class UsuarioResponseDto
    {
        public string? Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Token { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }

    }
}

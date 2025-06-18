namespace SistemaAutenticacionAPI.Dtos.UsuarioDtos
{
    /// <summary>
    /// Manejo de datos enviados desde el frontend
    /// </summary>
    public class UsuarioRegistroRequestDto
    {
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Telefono { get; set; }
        public string? Username { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}

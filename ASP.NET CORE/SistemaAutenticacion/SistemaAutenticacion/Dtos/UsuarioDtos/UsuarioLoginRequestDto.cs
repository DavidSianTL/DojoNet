namespace SistemaAutenticacion.Dtos.UsuarioDtos
{
    /// <summary>
    /// Manejo de datos enviados desde el frontend
    /// </summary>
    public class UsuarioLoginRequestDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}

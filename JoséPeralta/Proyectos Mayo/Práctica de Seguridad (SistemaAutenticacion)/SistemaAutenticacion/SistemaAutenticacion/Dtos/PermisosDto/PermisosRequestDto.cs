namespace SistemaAutenticacion.Dtos.PermisosDto
{
    public class PermisosRequestDto
    {
        public int Id { get; set; }
        public string? NombrePermiso { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Estado { get; set; } = true;
        public string? UsuarioCreacion { get; set; }
        public string? UsuarioModificacion { get; set; }
    }
}

namespace SistemaAutenticacionAPI.Dtos.RolesDtos
{
    public class RolRegistroRequestDto
    {
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaCreacion { get; set; }
    }
}

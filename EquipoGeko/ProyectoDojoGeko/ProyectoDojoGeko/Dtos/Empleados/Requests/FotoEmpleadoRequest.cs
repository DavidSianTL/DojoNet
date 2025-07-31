namespace ProyectoDojoGeko.Dtos.Empleados.Requests
{
    public class FotoEmpleadoRequest
    {
        public int IdEmpleado { get; set; }

        // Usamos IFormFile para manejar archivos subidos
        // Ya que ASP.NET Core maneja archivos subidos de esta manera
        public IFormFile FotoPerfil { get; set; }
    }

}

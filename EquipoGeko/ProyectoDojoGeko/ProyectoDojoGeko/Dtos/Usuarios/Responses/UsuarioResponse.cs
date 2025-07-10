using System;
using System.Collections.Generic;
using ProyectoDojoGeko.Dtos.Empleados.Responses;

namespace ProyectoDojoGeko.Dtos.Usuarios.Responses
{
    /// <summary>
    /// Representa la respuesta con la información de un usuario
    /// </summary>
    public class UsuarioResponse
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaExpiracionContrasenia { get; set; }
        public int EstadoId { get; set; }
        public string Estado { get; set; } = string.Empty;
        public EmpleadoResponse? Empleado { get; set; }
        public List<string> Roles { get; set; } = new List<string>();

        // Propiedades calculadas o de conveniencia
        public bool RequiereCambioContrasenia => 
            FechaExpiracionContrasenia.HasValue && 
            FechaExpiracionContrasenia.Value <= DateTime.UtcNow.AddDays(7);
    }
}

using System.Collections.Generic;
using System.Linq;


namespace ProyectoDojoGeko.Models
{
    public class AlertaEmpleadoDto
    {
        public int IdEmpleado { get; set; }
        public string NombresEmpleado { get; set; }
        public string ApellidosEmpleado { get; set; }
        public string Codigo { get; set; }
        public string TipoNotificacion { get; set; } // "Vacaciones por acumular (sin tomar)" o "Empleado próximo a salir (con vacaciones tomadas)"
    }

    public class ModeloTablero
    {
        // Propiedades existentes para tus otras tarjetas de estadísticas
        public int EmpresasActivas { get; set; }
        public int CambioEmpresasMes { get; set; }
        public int UsuariosTotales { get; set; }
        public int CambioUsuariosSemana { get; set; }
        public int SistemasRegistrados { get; set; }

        // Nueva propiedad para las alertas de empleados
        public List<AlertaEmpleadoDto> AlertasEmpleados { get; set; } = new List<AlertaEmpleadoDto>();

        // La cantidad total de alertas de seguridad se calcula a partir de la lista
        public int AlertasSeguridad => AlertasEmpleados.Count;
    }
}

using System.ComponentModel.DataAnnotations;

namespace ProyectoDojoGeko.Models
{
    public class DashboardViewModel
    {
        // Estadísticas de Empresas
        public int EmpresasActivas { get; set; }
        public int EmpresasTotal { get; set; }
        public int CambioEmpresasMes { get; set; }

        // Estadísticas de Usuarios
        public int UsuariosTotales { get; set; }
        public int UsuariosActivos { get; set; }
        public int UsuariosPendientes { get; set; }
        public int CambioUsuariosSemana { get; set; }

        // Estadísticas de Sistemas
        public int SistemasRegistrados { get; set; }
        public int SistemasTotal { get; set; }
        public int SistemasActivos { get; set; }

        // Estadísticas de Empleados
        public int EmpleadosActivos { get; set; }
        public int EmpleadosTotal { get; set; }
        public int EmpleadosSinUsuario { get; set; }

        // Alertas y Notificaciones
        public int AlertasSeguridad { get; set; }
        public int NotificacionesPendientes { get; set; }

        // NUEVA PROPIEDAD: Lista de alertas de empleados para vacaciones
        public List<AlertaEmpleadoVacacionesViewModel> AlertasEmpleadosVacaciones { get; set; } = new List<AlertaEmpleadoVacacionesViewModel>();

        // Actividades Recientes
        public List<BitacoraViewModel> ActividadesRecientes { get; set; } = new List<BitacoraViewModel>();

        // Datos para gráficos (opcional)
        public Dictionary<string, int> UsuariosPorRol { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ActividadesPorDia { get; set; } = new Dictionary<string, int>();

        // Métodos de cálculo
        public double PorcentajeEmpresasActivas => EmpresasTotal > 0 ? (double)EmpresasActivas / EmpresasTotal * 100 : 0;
        public double PorcentajeUsuariosActivos => UsuariosTotales > 0 ? (double)UsuariosActivos / UsuariosTotales * 100 : 0;
        public double PorcentajeSistemasActivos => SistemasTotal > 0 ? (double)SistemasActivos / SistemasTotal * 100 : 0;

        // Propiedades para mostrar tendencias
        public string TendenciaEmpresas => CambioEmpresasMes > 0 ? "positive" : CambioEmpresasMes < 0 ? "negative" : "neutral";
        public string TendenciaUsuarios => CambioUsuariosSemana > 0 ? "positive" : CambioUsuariosSemana < 0 ? "negative" : "neutral";

        // NUEVA PROPIEDAD CALCULADA: Total de alertas de empleados por vacaciones
        public int TotalAlertasEmpleadosVacaciones => AlertasEmpleadosVacaciones?.Count ?? 0;
    }

    // NUEVO MODELO: Para las alertas específicas de empleados y vacaciones
    public class AlertaEmpleadoVacacionesViewModel
    {
        public int IdEmpleado { get; set; }
        public string NombresEmpleado { get; set; } = string.Empty;
        public string ApellidosEmpleado { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string TipoNotificacion { get; set; } = string.Empty;
        public DateTime FechaIngreso { get; set; }

        // Campos para el cálculo completo de vacaciones
        public decimal AniosTrabajados { get; set; }
        public decimal DiasAcumuladosTotal { get; set; }
        public decimal DiasYaTomados { get; set; }
        public decimal DiasDisponibles { get; set; }

        // Propiedad calculada para mostrar el nombre completo
        public string NombreCompleto => $"{NombresEmpleado} {ApellidosEmpleado}".Trim();

        // Propiedad para mostrar información detallada según el tipo
        public string InformacionDetallada => TipoNotificacion.Contains("más de 14 días")
            ? $"Años trabajados: {AniosTrabajados:F1} | Acumulados: {DiasAcumuladosTotal:F1} | Tomados: {DiasYaTomados:F1} | Disponibles: {DiasDisponibles:F1}"
            : "Empleado próximo a salir con vacaciones tomadas";

        public string ResumenVacaciones => TipoNotificacion.Contains("más de 14 días")
            ? $"{DiasDisponibles:F1} días disponibles"
            : "Revisar situación laboral";
    }

    // Modelo para actividades recientes con información adicional
    public class ActividadRecienteViewModel
    {
        public int IdBitacora { get; set; }
        public DateTime FechaEntrada { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string NombreSistema { get; set; } = string.Empty;
        public string Accion { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string TipoAccion { get; set; } = string.Empty; // create, edit, delete, login, etc.
        public string IconoAccion { get; set; } = string.Empty;
        public string ColorAccion { get; set; } = string.Empty;

        // Tiempo relativo (ej: "Hace 5 minutos")
        public string TiempoRelativo
        {
            get
            {
                var diferencia = DateTime.Now - FechaEntrada;
                if (diferencia.TotalMinutes < 1)
                    return "Hace unos segundos";
                else if (diferencia.TotalMinutes < 60)
                    return $"Hace {(int)diferencia.TotalMinutes} min";
                else if (diferencia.TotalHours < 24)
                    return $"Hace {(int)diferencia.TotalHours} hora{((int)diferencia.TotalHours > 1 ? "s" : "")}";
                else
                    return $"Hace {(int)diferencia.TotalDays} día{((int)diferencia.TotalDays > 1 ? "s" : "")}";
            }
        }
    }
}
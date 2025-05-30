using Microsoft.AspNetCore.Mvc;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Filters;

namespace ProyectoDojoGeko.Controllers
{
    // [AuthorizeSession]  // ← Comenta esta línea temporalmente
    public class DashboardController : Controller
    {
        // Instanciamos todos los DAOs
        private readonly daoEmpresaWSAsync _daoEmpresa;
        private readonly daoUsuarioWSAsync _daoUsuario;
        private readonly daoSistemaWSAsync _daoSistema;
        private readonly daoBitacoraWSAsync _daoBitacora;
        private readonly daoEmpleadoWSAsync _daoEmpleado;

        // Constructor para inicializar las cadenas de conexión
        public DashboardController()
        {
            // Cadena de conexión a la base de datos - ACTUALIZADA
            string connectionString = "Server=DARLA\\SQLEXPRESS;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";

            // Inicializamos todos los DAOs con la cadena de conexión
            _daoEmpresa = new daoEmpresaWSAsync(connectionString);
            _daoUsuario = new daoUsuarioWSAsync(connectionString);
            _daoSistema = new daoSistemaWSAsync(connectionString);
            _daoBitacora = new daoBitacoraWSAsync(connectionString);
            _daoEmpleado = new daoEmpleadoWSAsync(connectionString);
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            try
            {
                // Crear el modelo del dashboard
                var dashboardModel = new DashboardViewModel();

                // Obtener estadísticas de empresas usando tu método real
                var empresas = await _daoEmpresa.ObtenerEmpresasAsync();
                dashboardModel.EmpresasActivas = empresas?.Count(e => e.Estado) ?? 0;
                dashboardModel.EmpresasTotal = empresas?.Count() ?? 0;

                // Calcular cambio de empresas este mes
                var empresasEsteMes = empresas?.Count(e => e.FechaCreacion.Month == DateTime.Now.Month &&
                                                          e.FechaCreacion.Year == DateTime.Now.Year) ?? 0;
                dashboardModel.CambioEmpresasMes = empresasEsteMes;

                // Obtener estadísticas de usuarios usando tu método real
                var usuarios = await _daoUsuario.ObtenerUsuariosAsync();
                dashboardModel.UsuariosTotales = usuarios?.Count() ?? 0;
                dashboardModel.UsuariosActivos = usuarios?.Count(u => u.Estado) ?? 0;

                // Calcular usuarios nuevos esta semana
                var inicioSemana = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
                var usuariosEstaSemana = usuarios?.Count(u => u.FechaCreacion >= inicioSemana) ?? 0;
                dashboardModel.CambioUsuariosSemana = usuariosEstaSemana;

                // Obtener estadísticas de sistemas usando tu método real
                var sistemas = await _daoSistema.ObtenerSistemasAsync();
                dashboardModel.SistemasRegistrados = sistemas?.Count(s => s.Estado) ?? 0;
                dashboardModel.SistemasTotal = sistemas?.Count() ?? 0;
                dashboardModel.SistemasActivos = sistemas?.Count(s => s.Estado) ?? 0;

                // Obtener estadísticas de empleados usando tu método real
                var empleados = await _daoEmpleado.ObtenerEmpleadoAsync();
                dashboardModel.EmpleadosActivos = empleados?.Count(e => e.Estado) ?? 0;
                dashboardModel.EmpleadosTotal = empleados?.Count() ?? 0;

                // Calcular empleados sin usuario asignado
                var empleadosConUsuario = usuarios?.Where(u => u.FK_IdEmpleado > 0).Select(u => u.FK_IdEmpleado).Distinct().ToList() ?? new List<int>();
                dashboardModel.EmpleadosSinUsuario = empleados?.Count(e => e.Estado && !empleadosConUsuario.Contains(e.IdEmpleado)) ?? 0;

                // Calcular alertas de seguridad
                var usuariosInactivos = usuarios?.Count(u => !u.Estado) ?? 0;
                dashboardModel.AlertasSeguridad = usuariosInactivos + dashboardModel.EmpleadosSinUsuario;

                // Obtener actividades recientes de la bitácora usando tu método real
                var todasLasBitacoras = await _daoBitacora.ObtenerBitacorasAsync();
                dashboardModel.ActividadesRecientes = todasLasBitacoras?
                    .OrderByDescending(b => b.FechaEntrada)
                    .Take(10)
                    .ToList() ?? new List<BitacoraViewModel>();

                // Pasar datos adicionales al ViewBag
                ViewBag.Usuario = HttpContext.Session.GetString("Usuario") ?? "Administrador";
                ViewBag.EmpresasActivas = dashboardModel.EmpresasActivas;
                ViewBag.UsuariosTotales = dashboardModel.UsuariosTotales;
                ViewBag.SistemasRegistrados = dashboardModel.SistemasRegistrados;
                ViewBag.AlertasSeguridad = dashboardModel.AlertasSeguridad;

                return View(dashboardModel);
            }
            catch (Exception ex)
            {
                // Log del error
                Console.WriteLine($"Error en Dashboard: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");

                // Crear modelo con datos por defecto en caso de error
                var defaultModel = new DashboardViewModel
                {
                    EmpresasActivas = 0,
                    UsuariosTotales = 0,
                    SistemasRegistrados = 0,
                    AlertasSeguridad = 0,
                    ActividadesRecientes = new List<BitacoraViewModel>()
                };

                ViewBag.Usuario = HttpContext.Session.GetString("Usuario") ?? "Administrador";
                ViewBag.Error = "Error al cargar los datos del dashboard. Por favor, verifique la conexión a la base de datos.";

                return View(defaultModel);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerEstadisticas()
        {
            try
            {
                var empresas = await _daoEmpresa.ObtenerEmpresasAsync();
                var usuarios = await _daoUsuario.ObtenerUsuariosAsync();
                var sistemas = await _daoSistema.ObtenerSistemasAsync();
                var empleados = await _daoEmpleado.ObtenerEmpleadoAsync();

                var estadisticas = new
                {
                    empresas = new
                    {
                        total = empresas?.Count() ?? 0,
                        activas = empresas?.Count(e => e.Estado) ?? 0,
                        inactivas = empresas?.Count(e => !e.Estado) ?? 0,
                        nuevasEsteMes = empresas?.Count(e => e.FechaCreacion.Month == DateTime.Now.Month &&
                                                            e.FechaCreacion.Year == DateTime.Now.Year) ?? 0
                    },
                    usuarios = new
                    {
                        total = usuarios?.Count() ?? 0,
                        activos = usuarios?.Count(u => u.Estado) ?? 0,
                        inactivos = usuarios?.Count(u => !u.Estado) ?? 0,
                        nuevosEstaSemana = usuarios?.Count(u => u.FechaCreacion >= DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek)) ?? 0
                    },
                    sistemas = new
                    {
                        total = sistemas?.Count() ?? 0,
                        activos = sistemas?.Count(s => s.Estado) ?? 0,
                        inactivos = sistemas?.Count(s => !s.Estado) ?? 0
                    },
                    empleados = new
                    {
                        total = empleados?.Count() ?? 0,
                        activos = empleados?.Count(e => e.Estado) ?? 0,
                        inactivos = empleados?.Count(e => !e.Estado) ?? 0
                    }
                };

                return Json(estadisticas);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerActividadesRecientes(int cantidad = 5)
        {
            try
            {
                var bitacoras = await _daoBitacora.ObtenerBitacorasAsync();
                var usuarios = await _daoUsuario.ObtenerUsuariosAsync();
                var sistemas = await _daoSistema.ObtenerSistemasAsync();

                var actividadesRecientes = new List<object>();

                if (bitacoras != null)
                {
                    actividadesRecientes = bitacoras
                        .OrderByDescending(b => b.FechaEntrada)
                        .Take(cantidad)
                        .Select(b => new
                        {
                            id = b.IdBitacora,
                            usuario = usuarios?.FirstOrDefault(u => u.IdUsuario == b.FK_IdUsuario)?.Username ?? $"Usuario #{b.FK_IdUsuario}",
                            accion = b.Accion,
                            descripcion = b.Descripcion,
                            sistema = sistemas?.FirstOrDefault(s => s.IdSistema == b.FK_IdSistema)?.Nombre ?? $"Sistema #{b.FK_IdSistema}",
                            fecha = b.FechaEntrada,
                            tiempoRelativo = CalcularTiempoRelativo(b.FechaEntrada)
                        })
                        .Cast<object>()
                        .ToList();
                }

                return Json(actividadesRecientes);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // Método auxiliar para calcular tiempo relativo
        private string CalcularTiempoRelativo(DateTime fecha)
        {
            var diferencia = DateTime.Now - fecha;

            if (diferencia.TotalMinutes < 1)
                return "Hace unos segundos";
            else if (diferencia.TotalMinutes < 60)
                return $"Hace {(int)diferencia.TotalMinutes} min";
            else if (diferencia.TotalHours < 24)
                return $"Hace {(int)diferencia.TotalHours} hora{((int)diferencia.TotalHours > 1 ? "s" : "")}";
            else if (diferencia.TotalDays < 30)
                return $"Hace {(int)diferencia.TotalDays} día{((int)diferencia.TotalDays > 1 ? "s" : "")}";
            else
                return fecha.ToString("dd/MM/yyyy");
        }
    }
}

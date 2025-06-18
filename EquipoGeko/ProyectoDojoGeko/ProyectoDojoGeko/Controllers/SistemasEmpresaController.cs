using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoDojoGeko.Data;
using ProyectoDojoGeko.Filters;
using ProyectoDojoGeko.Models;
using ProyectoDojoGeko.Models.RolPermisos;
using ProyectoDojoGeko.Models.SistemasEmpresa;
using ProyectoDojoGeko.Models.Usuario;

namespace ProyectoDojoGeko.Controllers
{
    [AuthorizeSession]
    public class SistemasEmpresaController : Controller
    {
        private readonly daoSistemasEmpresaWSAsync _daoSistemasEmpresa;
        private readonly daoSistemaWSAsync _daoSistemas;
        private readonly daoEmpresaWSAsync _daoEmpresas;
        private readonly daoBitacoraWSAsync _daoBitacora;
        private readonly daoLogWSAsync _daoLog;

        public SistemasEmpresaController()
        {
            // Cadena de conexión a la base de datos
            string _connectionString = "Server=db20907.public.databaseasp.net;Database=db20907;User Id=db20907;Password=A=n95C!b#3aZ;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=True;";
            //string _connectionString = "Server=NEWPEGHOSTE\\SQLEXPRESS;Database=DBProyectoGrupalDojoGeko;Trusted_Connection=True;TrustServerCertificate=True;";
            // Inicializa el DAO con la cadena de conexión
            _daoSistemasEmpresa = new daoSistemasEmpresaWSAsync(_connectionString);
            _daoSistemas = new daoSistemaWSAsync(_connectionString);
            _daoEmpresas = new daoEmpresaWSAsync(_connectionString);
            _daoBitacora = new daoBitacoraWSAsync(_connectionString);
            _daoLog = new daoLogWSAsync(_connectionString);
        }

        // Método privado para registrar errores en Log
        private async Task RegistrarError(string accion, Exception ex)
        {
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            await _daoLog.InsertarLogAsync(new LogViewModel
            {
                Accion = $"Error {accion}",
                Descripcion = $"Error al {accion} por {usuario}: {ex.Message}",
                Estado = false
            });
        }

        // Método privado para registrar acciones exitosas en Bitácora
        private async Task RegistrarBitacora(string accion, string descripcion)
        {
            var idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
            var usuario = HttpContext.Session.GetString("Usuario") ?? "Sistema";
            var idSistema = HttpContext.Session.GetInt32("IdSistema") ?? 0;

            await _daoBitacora.InsertarBitacoraAsync(new BitacoraViewModel
            {
                Accion = accion,
                Descripcion = descripcion,
                FK_IdUsuario = idUsuario,
                FK_IdSistema = idSistema
            });
        }

            [HttpGet]
            [AuthorizeRole("SuperAdministrador", "Administrador", "Editor", "Visualizador")]
            public async Task<IActionResult> Index()
            {
                try
                {
                    var sistemasEmpresa = await _daoSistemasEmpresa.ObtenerSistemasEmpresaAsync();

                    // Verificar si la lista está vacía
                    if (sistemasEmpresa == null || sistemasEmpresa.Count == 0)
                    {
                        TempData["Error"] = "No hay relaciones de sistemas a empresa disponibles.";
                        return View(new List<SistemasEmpresaFormViewModel>());
                    }

                    // Registrar bitácora, aunque no haya datos
                    await RegistrarBitacora("Vista Relación Sistemas a Empresa", "Ingreso a la vista de relación de sistemas a empresa");

                    // Retornar la vista con los datos obtenidos
                    return View(sistemasEmpresa);
                }
                catch (Exception e)
                {
                    await RegistrarError("Error Vista Relación Sistemas a Empresa", e);
                    return View(new List<SistemasEmpresaFormViewModel>());
                }
            }


        // Vista para crear una nueva relación de sistema a empresa
        [HttpGet]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear()
        {
            // Obtenemos las listas de empresas y sistemas desde los DAOs
            var empresas = await _daoEmpresas.ObtenerEmpresasAsync();
            var sistemas = await _daoSistemas.ObtenerSistemasAsync();

            // Preparamos el modelo para la vista
            var model = new SistemasEmpresaFormViewModel
            {
                // Asignamos la lista de usuarios para el dropdown
                Empresas = empresas?.Select(u => new SelectListItem
                {
                    Value = u.IdEmpresa.ToString(),
                    Text = u.Nombre
                }).ToList() ?? new List<SelectListItem>(),

                // Asignamos la lista de roles para el dropdown
                Sistemas = sistemas?.Select(r => new SelectListItem
                {
                    Value = r.IdSistema.ToString(),
                    Text = r.Nombre
                }).ToList() ?? new List<SelectListItem>()
            };

            // Verificamos si hay empresas y sistemas disponibles para asignar
            if (model.Empresas.Count == 0 || model.Sistemas.Count == 0)
            {
                TempData["Error"] = "No hay empresas o sistemas disponibles para asignar. Por favor, asegúrese de que existan registros en las tablas correspondientes.";
                return RedirectToAction(nameof(Index));
            }

            // Enviamos a la bitácora el ingreso a la vista de relación de sistemas a empresa
            await RegistrarBitacora("Vista Relación Sistemas a Empresa", "Creación de una nueva relación de sistemas a empresa");

            // Retorna la vista con el modelo preparado
            return View(model);

        }

        // Acción para crear una nueva relación de sistema a empresa
        [HttpPost]
        [AuthorizeRole("SuperAdministrador", "Administrador", "Editor")]
        public async Task<IActionResult> Crear(SistemasEmpresaFormViewModel model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    model.Empresas = (await _daoEmpresas.ObtenerEmpresasAsync()).Select(e => new SelectListItem
                    {
                        Value = e.IdEmpresa.ToString(),
                        Text = e.Nombre
                    }).ToList();

                    model.Sistemas = (await _daoSistemas.ObtenerSistemasAsync()).Select(s => new SelectListItem
                    {
                        Value = s.IdSistema.ToString(),
                        Text = s.Nombre
                    }).ToList();

                    foreach (var key in ModelState.Keys)
                    {
                        var errors = ModelState[key].Errors;
                        foreach (var error in errors)
                        {
                            // Puedes poner un breakpoint aquí o loguear el error
                            Console.WriteLine($"Campo: {key}, Error: {error.ErrorMessage}");
                        }
                    }

                    return View(model);
                }

                foreach (int sistemasId in model.FK_IdsSistema)
                {
                    // Convertimos SistemasEmpresaFormViewModel a SistemasEmpresaViewModel
                    var nuevoSistemaEmpresa = new SistemasEmpresaViewModel
                    {
                        FK_IdEmpresa = model.SistemasEmpresa.FK_IdEmpresa,
                        FK_IdSistema = sistemasId // Asignamos el ID del sistema directamente
                    };

                    await _daoSistemasEmpresa.InsertarSistemasEmpresaAsync(nuevoSistemaEmpresa);
                }

                await RegistrarBitacora("CrearRolPermisos", "Rol y permisos creados exitosamente");
                TempData["SuccessMessage"] = "Rol y permisos creados correctamente.";
                return RedirectToAction(nameof(Crear));
            }
            catch (Exception e)
            {
                await RegistrarError("Crear ", e);
                ModelState.AddModelError(string.Empty, "Error al crear la relación: " + e.Message);
                return View(model);
            }
        }


    }
}

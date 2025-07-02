using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using _Evaluacion_Mensual_Abril.Models.Binnacle;
using _Evaluacion_Mensual_Abril.Services.FakeStore;

namespace _Evaluacion_Mensual_Abril.Controllers
{
    public class BinnacleController : Controller
    {
        private readonly string _rutaLog = "log.txt";

        // Función global para obtener el nombre completo del usuario
        public string NombreCompletoLog()
        {
            var nombreCompleto = HttpContext.Session.GetString("NombreCompleto");
            return nombreCompleto != null ? $"[Usuario: {nombreCompleto}]" : "[Usuario: No identificado]";
        }

        // Función para registrar en log.txt
        private void RegistrarLog(string accion, string descripcion)
        {
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {NombreCompletoLog()} [Acción: {accion}] {descripcion}{Environment.NewLine}";
            System.IO.File.AppendAllText("log.txt", logEntry);
        }

        public async Task<IActionResult> Binnacle()
        {
            var logs = new List<BinnacleViewModel>();

            var userNombre = HttpContext.Session.GetString("UsrNombre");
            var nombreCompleto = HttpContext.Session.GetString("NombreCompleto");

            if (userNombre != null)
            {
                ViewBag.usrNombre = userNombre;
                ViewBag.NombreCompleto = nombreCompleto;

                var isLoggedIn = HttpContext.Session.GetString("UsrNombre") != null;
                ViewData["isLoggedIn"] = isLoggedIn;

                if (System.IO.File.Exists(_rutaLog))
                {
                    var lineas = System.IO.File.ReadAllLines(_rutaLog);
                    var regex = new Regex(@"^(?<fecha>\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}) \[Usuario: (?<usuario>.*?)\] \[Acción: (?<accion>.*?)\] (?<descripcion>.*)$");

                    foreach (var linea in lineas)
                    {
                        var match = regex.Match(linea);
                        if (match.Success)
                        {
                            logs.Add(new BinnacleViewModel
                            {
                                FechaHora = match.Groups["fecha"].Value,
                                Usuario = match.Groups["usuario"].Value,
                                Accion = match.Groups["accion"].Value,
                                Descripcion = match.Groups["descripcion"].Value
                            });
                        }
                    }
                }

                var modelo = new BinnaclePageViewModel
                {
                    Logs = logs
                };

                return View(modelo);

            }
            else
            {
                RegistrarLog("Acceso Bitácora", "Error: Usuario no autenticado al intentar acceder a la vista de bitácora.");
                return RedirectToAction("Login", "Login");

            }

            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(BinnaclePageViewModel model)
        {
            var entrada = model.NuevaEntrada;
            var usuario = HttpContext.Session.GetString("NombreCompleto") ?? "No identificado";
            var nuevaLinea = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [Usuario: {usuario}] [Acción: {entrada.NuevaAccion}] {entrada.NuevaDescripcion}{Environment.NewLine}";
            System.IO.File.AppendAllText(_rutaLog, nuevaLinea);

            return RedirectToAction("Binnacle");
        }

    }
}

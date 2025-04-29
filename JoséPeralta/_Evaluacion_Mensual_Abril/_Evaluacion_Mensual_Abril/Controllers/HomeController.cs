using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using _Evaluacion_Mensual_Abril.Models;
using System.IO;
using System.Threading.Tasks;

namespace _Evaluacion_Mensual_Abril.Controllers
{
    public class HomeController : Controller
    {
        // Función global para obtener el nombre completo del usuario
        private string NombreCompletoLog()
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

        public async Task<IActionResult> Index()
        {
            var userNombre = HttpContext.Session.GetString("UsrNombre");
            var nombreCompleto = HttpContext.Session.GetString("NombreCompleto");
            var mostrarAlerta = HttpContext.Session.GetString("MostrarAlerta");

            try
            {
                if (userNombre != null)
                {
                    ViewBag.usrNombre = userNombre;
                    ViewBag.NombreCompleto = nombreCompleto;

                    var isLoggedIn = HttpContext.Session.GetString("UsrNombre") != null;
                    ViewData["isLoggedIn"] = isLoggedIn;

                    // Solo mostrar la alerta si "MostrarAlerta" está configurado
                    if (mostrarAlerta == "true")
                    {
                        ViewBag.MostrarAlerta = true;
                        HttpContext.Session.SetString("MostrarAlerta", "false"); // Desactivar la alerta
                    }
                    else
                    {
                        ViewBag.MostrarAlerta = false;
                    }

                    RegistrarLog("Acceso Index", "Acceso correcto a la vista de inicio.");
                    return View();
                }
                else
                {
                    RegistrarLog("Acceso Index", "Error: Usuario no autenticado al intentar acceder a la vista de inicio.");
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception e)
            {
                RegistrarLog("Acceso Index", $"Error inesperado al cargar la vista de inicio. Detalle: {e.Message}");
                return RedirectToAction("Login", "Login");
            }
        }




    }
}

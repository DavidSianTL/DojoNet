using Microsoft.AspNetCore.Mvc;
using _Evaluacion_Mensual_Abril.Models;
using _Evaluacion_Mensual_Abril.Services.TempConvert;

namespace _Evaluacion_Mensual_Abril.Controllers
{
    public class TPController : Controller
    {
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

        // Acción que maneja la vista de la API SOAP
        public IActionResult Index()
        {
            try
            {
                var userNombre = HttpContext.Session.GetString("UsrNombre");
                var nombreCompleto = HttpContext.Session.GetString("NombreCompleto");

                if (userNombre != null)
                {
                    ViewBag.usrNombre = userNombre;
                    ViewBag.NombreCompleto = nombreCompleto;
                    ViewData["isLoggedIn"] = true;

                    RegistrarLog("Acceso a la API SOAP", "Acceso correcto a la vista de la API SOAP.");
                    return View("~/Views/TempConvert/Index.cshtml");
                }
                else
                {
                    RegistrarLog("Acceso Denegado", "Intento de acceder sin sesión activa.");
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception e)
            {
                RegistrarLog("Error", $"Exception en Products(): {e.Message}");
                return RedirectToAction("Login", "Login");
            }
        }

        // Acción que maneja la conversión de temperatura
        [HttpPost]
        public async Task<IActionResult> ConvertTemperature(string tipo, string valor)
        {
            try
            {
                var servicio = new TPService(); // Idealmente usar inyección de dependencias
                string resultado = "";

                if (tipo == "CtoF")
                {
                    resultado = await servicio.CelsiusToFahrenheitAsync(valor);
                    RegistrarLog("Conversión Temperatura", $"Celsius a Fahrenheit. Valor: {valor}, Resultado: {resultado}");
                }
                else if (tipo == "FtoC")
                {
                    resultado = await servicio.FahrenheitToCelsiusAsync(valor);
                    RegistrarLog("Conversión Temperatura", $"Fahrenheit a Celsius. Valor: {valor}, Resultado: {resultado}");
                }
                else
                {
                    RegistrarLog("Conversión Temperatura", $"Tipo inválido: {tipo}");
                    ViewBag.Error = "Tipo de conversión inválido.";
                }

                ViewBag.Resultado = resultado;
                ViewBag.Tipo = tipo;
                ViewBag.Valor = valor;

                return View("~/Views/TempConvert/Index.cshtml");
            }
            catch (Exception e)
            {
                RegistrarLog("Error", $"Exception en ConvertTemperature(): {e.Message}");
                return RedirectToAction("Index");
            }
        }






    }
}

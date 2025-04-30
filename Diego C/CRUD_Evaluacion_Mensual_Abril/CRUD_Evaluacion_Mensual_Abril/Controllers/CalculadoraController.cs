using Microsoft.AspNetCore.Mvc;
using Calculadora;
using System.Threading.Tasks;
using CRUD_Evaluacion_Mensual_Abril.Services;
using Microsoft.AspNetCore.Http;
using CRUD_Evaluacion_Mensual_Abril.Models;

namespace CRUD_Evaluacion_Mensual_Abril.Controllers
{
    public class CalculadoraController : Controller
    {
        private readonly BitacoraService _bitacora;

        public CalculadoraController(BitacoraService bitacora)
        {
            _bitacora = bitacora;
        }

        public IActionResult Index()
        {
            var usrNombre = HttpContext.Session.GetString("UsrNombre");
            if (usrNombre == null)
            {
                return RedirectToAction("Login", "Login"); // Redirige a la página de login si no está autenticado
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Sumar(int? num1, int? num2)
        {
            var usrNombre = HttpContext.Session.GetString("UsrNombre");

            if (usrNombre == null)
            {
                return RedirectToAction("Login", "Login"); // Redirige a la página de login si no está autenticado
            }

            // Verificar que los valores sean válidos
            if (!num1.HasValue || !num2.HasValue)
            {
                TempData["Error"] = "Debe ingresar ambos números.";
                return View();
            }

            try
            {
                var client = new CalculatorSoapClient(CalculatorSoapClient.EndpointConfiguration.CalculatorSoap);
                var result = await client.AddAsync(num1.Value, num2.Value);
                await client.CloseAsync();

                ViewBag.Resultado = result;
                _bitacora.RegistrarEvento(HttpContext, usrNombre, $"Realizó una suma: {num1} + {num2} = {result}  desde servicio ApiSOAP");
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al consumir el servicio: {ex.Message} ";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Restar(int? num1, int? num2)
        {
            var usrNombre = HttpContext.Session.GetString("UsrNombre");

            if (usrNombre == null)
            {
                return RedirectToAction("Login", "Login"); // Redirige a la página de login si no está autenticado
            }

            // Verificar que los valores sean válidos
            if (!num1.HasValue || !num2.HasValue)
            {
                TempData["Error"] = "Debe ingresar ambos números.";
                return View();
            }

            try
            {
                var client = new CalculatorSoapClient(CalculatorSoapClient.EndpointConfiguration.CalculatorSoap);
                var result = await client.SubtractAsync(num1.Value, num2.Value);
                await client.CloseAsync();

                ViewBag.Resultado = result;
                _bitacora.RegistrarEvento(HttpContext, usrNombre, $"Realizó una resta: {num1} - {num2} = {result} desde servicio ApiSOAP");
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al consumir el servicio: {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Multiplicar(int? num1, int? num2)
        {
            var usrNombre = HttpContext.Session.GetString("UsrNombre");

            if (usrNombre == null)
            {
                return RedirectToAction("Login", "Login"); // Redirige a la página de login si no está autenticado
            }

            // Verificar que los valores sean válidos
            if (!num1.HasValue || !num2.HasValue)
            {
                TempData["Error"] = "Debe ingresar ambos números.";
                return View();
            }

            try
            {
                var client = new CalculatorSoapClient(CalculatorSoapClient.EndpointConfiguration.CalculatorSoap);
                var result = await client.MultiplyAsync(num1.Value, num2.Value);
                await client.CloseAsync();

                ViewBag.Resultado = result;
                _bitacora.RegistrarEvento(HttpContext, usrNombre, $"Realizó una multiplicación: {num1} * {num2} = {result} desde servicio ApiSOAP");
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al consumir el servicio: {ex.Message}";
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Dividir(int? num1, int? num2)
        {
            var usrNombre = HttpContext.Session.GetString("UsrNombre");

            if (usrNombre == null)
            {
                return RedirectToAction("Login", "Login"); // Redirige a la página de login si no está autenticado
            }

            // Verificar que los valores sean válidos
            if (!num1.HasValue || !num2.HasValue)
            {
                TempData["Error"] = "Debe ingresar ambos números.";
                return View();
            }

            if (num2 == 0)
            {
                TempData["Error"] = "No se puede dividir entre cero.";
                _bitacora.RegistrarEvento(HttpContext, usrNombre, "Intentó dividir entre cero");
                return View();
            }

            try
            {
                var client = new CalculatorSoapClient(CalculatorSoapClient.EndpointConfiguration.CalculatorSoap);
                var result = await client.DivideAsync(num1.Value, num2.Value);
                await client.CloseAsync();

                ViewBag.Resultado = result;
                _bitacora.RegistrarEvento(HttpContext, usrNombre, $"Realizó una división: {num1} / {num2} = {result}  desde servicio ApiSOAP");
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al consumir el servicio: {ex.Message}";
                return View();
            }
        }
    }
}

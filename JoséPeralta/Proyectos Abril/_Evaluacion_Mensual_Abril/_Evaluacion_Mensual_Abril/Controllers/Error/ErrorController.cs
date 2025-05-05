using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using _Evaluacion_Mensual_Abril.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace _Evaluacion_Mensual_Abril.Controllers.Error
{
    public class ErrorController : Controller
    {
  
        [Route("Error/Error")]
        public IActionResult Error(ErrorViewModel error, int? statusCode)   
        {

            // Si no se proporciona un modelo de error, crear uno nuevo
            if (error == null)
            {
                error = new ErrorViewModel
                {
                    RequestId = HttpContext.TraceIdentifier
                };
            }

            if (statusCode == 404)
            {
                ViewData["Title"] = "Error 404 - Página no encontrada";
                ViewData["ErrorMessage"] = "La página solicitada no fue encontrada.";
                return View("~/Views/Shared/Error.cshtml", error); // Pasar el modelo completo
            }

            ViewData["Title"] = "Error";
            ViewData["ErrorMessage"] = "Ocurrió un error inesperado.";
            return View("~/Views/Shared/NotFound.cshtml", error); // Pasar el modelo completo
        }

    }
}

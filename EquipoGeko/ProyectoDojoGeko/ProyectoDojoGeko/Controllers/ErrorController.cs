using Microsoft.AspNetCore.Mvc;

namespace ProyectoDojoGeko.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult Error404()
        {
            Response.StatusCode = 404;
            return View("Error404");
        }

        [Route("Error/403")] // Nueva ruta para el error 403
        public IActionResult Error403()
        {
            Response.StatusCode = 403;
            return View("Error403"); // Llama a la nueva vista Error403.cshtml
        }

        [Route("Error/{code:int}")]
        public IActionResult Error(int code)
        {
            Response.StatusCode = code;

            switch (code)
            {
                case 404:
                    return View("Error404");
                case 403:
                    return View("Error403"); // Añadido caso para 403
                // Puedes añadir mas casos para otros codigos de error aqui
                // case 500:
                //     return View("Error500");
                default:
                    // Por defecto, si no es un error especifico, muestra la pagina 404 o una generica
                    return View("Error404");
            }
        }
    }
}

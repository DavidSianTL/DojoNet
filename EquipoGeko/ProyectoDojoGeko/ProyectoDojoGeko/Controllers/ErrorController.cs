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

        [Route("Error/{code:int}")]
        public IActionResult Error(int code)
        {
            Response.StatusCode = code;

            switch (code)
            {
                case 404:
                    return View("Error404");
                default:
                    return View("Error404");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace ExamenUno.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/404")]
        public IActionResult Error404()
        {

            return View("NotFound");
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Reto.Controllers.Producto
{
    public class ProductoController : Controller
    {
        [HttpGet]
        public IActionResult Producto()
        {
            //si el usuario es valido estará guardado en la sesion
            string usuario = HttpContext.Session.GetString("Usuario");

            //si es null o vacío entonces no lo hemos guardado; si no lo hemos guardado entonces no era valido/ o no inició sesión
            if (string.IsNullOrEmpty(usuario)) return RedirectToAction("Login", "Login"); // y lo redirigimos a la vista login

            //si está en la sesión lo redirigimos a la vista Producto
            return View();
        }
    }
}

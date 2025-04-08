using FormularioJS.Models;
using Microsoft.AspNetCore.Mvc;
using FormularioJS.Models;

namespace FormularioJS.Controllers
{
    public class PedidoController : Controller
    {
        public class ProductoController : Controller
        {
            // Acción GET para mostrar el formulario
            [HttpGet]
            public IActionResult Formulario()
            {
                return View(Formulario);
            }
        }

        // Acción POST para procesar los datos del formulario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Formulario(PedidoModel modelo)
        {
            if (ModelState.IsValid)
            {
                // Procesar el pedido (guardar en base de datos, etc.)
                return RedirectToAction("Success");  // Redirige a una página de éxito
            }

            // Si el modelo no es válido, vuelve a mostrar el formulario con errores
            return View(modelo);
        }
    }
}
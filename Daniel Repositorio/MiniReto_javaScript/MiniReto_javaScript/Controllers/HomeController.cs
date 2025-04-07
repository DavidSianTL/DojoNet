using Microsoft.AspNetCore.Mvc;
using MiniReto_javaScript.Models;

public class HomeController : Controller
{
    // Mostrar formulario
    public IActionResult Index()
    {
        return View(new FormularioTienda()); // Aseg�rate de pasar el modelo
    }

    // Procesar formulario
    [HttpPost]
    public IActionResult Submit(FormularioTienda formulario)
    {
        if (ModelState.IsValid)
        {
            // Vista de �xito
            return View("Exito");
        }

        // Volver al formulario con errores
        return View("Index", formulario);
    }
}

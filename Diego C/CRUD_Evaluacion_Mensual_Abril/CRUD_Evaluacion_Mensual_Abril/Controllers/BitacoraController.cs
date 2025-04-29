using CRUD_Evaluacion_Mensual_Abril.Models;
using Microsoft.AspNetCore.Mvc;

public class BitacoraController : Controller
{
    private readonly string _bitacoraFilePath = Path.Combine(Directory.GetCurrentDirectory(), "bitacora.txt");

    public IActionResult Index()
    {

        var usrNombre = HttpContext.Session.GetString("UsrNombre");
        var NombreCompleto = HttpContext.Session.GetString("NombreCompleto");

        if (usrNombre == null)
        {
            return RedirectToAction("Login", "Login");
        }

        ViewBag.usrNombre = usrNombre;
        ViewBag.NombreCompleto = NombreCompleto;

        var registros = System.IO.File.ReadAllLines(_bitacoraFilePath);
        return View(registros);
    }

    // Acción para agregar una nueva entrada
    [HttpPost]
    public IActionResult AgregarRegistro(string mensaje)
    {
        var usrNombre = HttpContext.Session.GetString("UsrNombre");
        var NombreCompleto = HttpContext.Session.GetString("NombreCompleto");

        if (usrNombre == null)
        {
            return RedirectToAction("Login", "Login");
        }

        ViewBag.usrNombre = usrNombre;
        ViewBag.NombreCompleto = NombreCompleto;
       

        if (!string.IsNullOrEmpty(mensaje))
        {
          
            var entrada = $"{DateTime.Now} - {usrNombre} - {mensaje}";
            System.IO.File.AppendAllText(_bitacoraFilePath, entrada + Environment.NewLine);
            TempData["Success"] = "Entrada agregada correctamente";
        }
        else
        {
            TempData["Error"] = "Por favor ingresa un mensaje válido";
        }

        return RedirectToAction("Index");
    }
}


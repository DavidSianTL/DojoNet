using Microsoft.AspNetCore.Mvc;
using ValidacionesCliente.Models;

namespace ValidacionesCliente.Controllers
{
    public IActionResult produtc()
    {
        var producto = new ProductModel(); // O llena los datos si es necesario
        return View(producto);
    }
}

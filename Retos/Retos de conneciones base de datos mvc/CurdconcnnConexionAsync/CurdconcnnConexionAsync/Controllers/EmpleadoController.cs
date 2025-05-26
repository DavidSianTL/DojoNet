using CurdconcnnConexionAsync.Models;
using Microsoft.AspNetCore.Mvc;

public class EmpleadoController : Controller
{
    private readonly EmpleadoDAO empleadoDAO = new EmpleadoDAO();

    public async Task<IActionResult> Index()
    {
        var empleados = await empleadoDAO.ObtenerEmpleadosAsync() ?? new List<Empleado>();
        return View(empleados);
    }

    [HttpPost]
    public async Task<IActionResult> Agregar(Empleado empleado)
    {
        if (empleado == null)
        {
            TempData["Error"] = "❌ Error: No se recibieron datos.";
            return RedirectToAction("Index");
        }

        try
        {
            await empleadoDAO.AgregarEmpleadoAsync(empleado);
            TempData["Success"] = "✅ Empleado agregado correctamente.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"❌ Error al agregar empleado: {ex.Message}";
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            if (id <= 0)
            {
                TempData["Error"] = "❌ Error: ID inválido.";
                return RedirectToAction("Index");
            }

            await empleadoDAO.EliminarEmpleadoAsync(id);
            TempData["Success"] = "✅ Empleado eliminado correctamente.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"❌ Error al eliminar empleado: {ex.Message}";
        }

        return RedirectToAction("Index");
    }
}
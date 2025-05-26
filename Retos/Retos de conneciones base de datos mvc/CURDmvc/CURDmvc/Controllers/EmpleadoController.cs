using CURDmvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

public class EmpleadoController : Controller
{
    private readonly EmpleadoDAO empleadoDAO = new EmpleadoDAO();

    // 🔹 Mostrar lista de empleados
    public IActionResult Index()
    {
        var empleados = empleadoDAO.ObtenerEmpleados() ?? new List<Empleado>(); // Evita valores nulos
        return View(empleados);
    }

    // 🔹 Agregar un nuevo empleado
    [HttpPost]
    public IActionResult Agregar(Empleado empleado)
    {
        // 🛠 Validar que `empleado` no sea null
        if (empleado == null)
        {
            TempData["Error"] = "❌ Error: No se recibieron datos del formulario.";
            return RedirectToAction("Index");
        }

        // 🖥 Mostrar los datos recibidos en la consola para depuración
        Console.WriteLine($"Datos recibidos -> Nombre: {empleado.Nombre}, Apellido: {empleado.Apellido}, Puesto: {empleado.Puesto}, SalarioBase: {empleado.SalarioBase}");

        // 🔎 Validar modelo antes de insertar
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "❌ Error: Datos inválidos. Verifica los campos.";
            return RedirectToAction("Index");
        }

        try
        {
            empleadoDAO.AgregarEmpleado(empleado);
            TempData["Success"] = "✅ Empleado agregado correctamente.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"❌ Error al agregar empleado: {ex.Message}";
        }

        return RedirectToAction("Index");
    }

    // 🔹 Eliminar un empleado
    [HttpPost]
    public IActionResult Eliminar(int id)
    {
        try
        {
            if (id <= 0)
            {
                TempData["Error"] = "❌ Error: ID de empleado inválido.";
                return RedirectToAction("Index");
            }

            empleadoDAO.EliminarEmpleado(id);
            TempData["Success"] = "✅ Empleado eliminado correctamente.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"❌ Error al eliminar empleado: {ex.Message}";
        }

        return RedirectToAction("Index");
    }
}

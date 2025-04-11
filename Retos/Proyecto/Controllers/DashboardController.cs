using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;
using Proyecto.Filters; // <--- Asegurate de importar tu filtro personalizado
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Proyecto.Controllers
{
    [RequireLogin] // <--- Aplica el filtro a todo el controlador
    public class DashboardController : Controller
    {
        private readonly string _jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "productos.json");

        public IActionResult Index()
        {
            // Leer los productos desde JSON
            var productos = GetProductos();

            if (productos == null || !productos.Any())
            {
                TempData["ErrorMessage"] = "No hay productos disponibles.";
            }

            return View(productos); // Pasar la lista de productos a la vista
        }

        private List<Producto> GetProductos()
        {
            if (!System.IO.File.Exists(_jsonPath))
            {
                // Crear archivo vacío si no existe
                System.IO.File.WriteAllText(_jsonPath, "[]");
                return new List<Producto>();
            }

            string json = System.IO.File.ReadAllText(_jsonPath);
            return JsonConvert.DeserializeObject<List<Producto>>(json) ?? new List<Producto>();
        }
    }
}

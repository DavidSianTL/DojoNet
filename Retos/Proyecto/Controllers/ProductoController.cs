// ... (usings iguales)
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto.Models;
using Proyecto.Utils;
using Proyecto.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Proyecto.Controllers
{
    [RequireLogin]
    public class ProductoController : Controller
    {
        private readonly string _jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "productos.json");

        private List<Producto> GetProductos()
        {
            try
            {
                if (!System.IO.File.Exists(_jsonPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_jsonPath));
                    System.IO.File.WriteAllText(_jsonPath, "[]");
                    return new List<Producto>();
                }

                string json = System.IO.File.ReadAllText(_jsonPath);
                var productos = JsonConvert.DeserializeObject<List<Producto>>(json) ?? new List<Producto>();
                return productos;
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("Leer Productos", ex);
                return new List<Producto>();
            }
        }

        private void SaveProductos(List<Producto> productos)
        {
            try
            {
                string json = JsonConvert.SerializeObject(productos, Formatting.Indented);
                System.IO.File.WriteAllText(_jsonPath, json);
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("Guardar Productos", ex);
                throw;
            }
        }

        public IActionResult Index()
        {
            try
            {
                var productos = GetProductos();
                if (productos == null || !productos.Any())
                {
                    TempData["ErrorMessage"] = "No hay productos disponibles.";
                }
                return View(productos);
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("Mostrar Productos", ex);
                TempData["ErrorMessage"] = "Error al cargar la lista de productos.";
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("Mostrar Vista Create", ex);
                TempData["ErrorMessage"] = "Error al cargar la vista de creación.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var productos = GetProductos();
                    producto.Id = productos.Count > 0 ? productos.Max(p => p.Id) + 1 : 1;
                    productos.Add(producto);
                    SaveProductos(productos);

                    Logger.RegistrarAccion(User.Identity.Name ?? "Anónimo", $"Creó producto: {producto.Nombre}");

                    TempData["SuccessMessage"] = $"Producto '{producto.Nombre}' creado exitosamente.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Logger.RegistrarError("Crear Producto", ex);
                    ModelState.AddModelError("", $"Error al crear producto: {ex.Message}");
                }
            }
            return View(producto);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var productos = GetProductos();
                var producto = productos.FirstOrDefault(p => p.Id == id);

                if (producto == null)
                {
                    TempData["ErrorMessage"] = "Producto no encontrado.";
                    return RedirectToAction("Index");
                }
                return View(producto);
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("Cargar Vista Edit", ex);
                TempData["ErrorMessage"] = "Error al cargar la vista.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Producto producto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var productos = GetProductos();
                    var productoExistente = productos.FirstOrDefault(p => p.Id == producto.Id);

                    if (productoExistente != null)
                    {
                        productoExistente.Nombre = producto.Nombre;
                        productoExistente.Precio = producto.Precio;
                        productoExistente.Cantidad = producto.Cantidad;
                        SaveProductos(productos);

                        Logger.RegistrarAccion(User.Identity.Name ?? "Anónimo", $"Editó producto: {producto.Nombre}");

                        TempData["SuccessMessage"] = $"Producto '{producto.Nombre}' actualizado exitosamente.";
                        return RedirectToAction("Index");
                    }
                    TempData["ErrorMessage"] = "Producto no encontrado.";
                }
                catch (Exception ex)
                {
                    Logger.RegistrarError("Editar Producto", ex);
                    ModelState.AddModelError("", $"Error al actualizar producto: {ex.Message}");
                }
            }
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                var productos = GetProductos();
                var producto = productos.FirstOrDefault(p => p.Id == id);

                if (producto != null)
                {
                    productos.Remove(producto);
                    SaveProductos(productos);

                    Logger.RegistrarAccion(User.Identity.Name ?? "Anónimo", $"Eliminó producto: {producto.Nombre}");

                    TempData["SuccessMessage"] = $"Producto '{producto.Nombre}' eliminado exitosamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Producto no encontrado.";
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("Eliminar Producto", ex);
                TempData["ErrorMessage"] = $"Error al eliminar producto: {ex.Message}";
            }
            return RedirectToAction("Index");
        }
    }
}

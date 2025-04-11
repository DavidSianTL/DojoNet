using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto.Models;
using Proyecto.Utils; // Importa la clase Logger
using Proyecto.Filters; // Agregar la directiva 'using' para el filtro
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Proyecto.Controllers
{
    [RequireLogin]  // Asegúrate de que el filtro sea reconocido
    public class ProductoController : Controller
    {
        private readonly string _jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "productos.json");

        // Método para leer productos desde JSON
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
                Logger.RegistrarError("Leer Productos", ex); // Registra el error usando Logger
                return new List<Producto>();
            }
        }

        // Método para guardar productos en JSON
        private void SaveProductos(List<Producto> productos)
        {
            try
            {
                string json = JsonConvert.SerializeObject(productos, Formatting.Indented);
                System.IO.File.WriteAllText(_jsonPath, json);
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("Guardar Productos", ex); // Registra el error usando Logger
                throw;
            }
        }

        // Mostrar la lista de productos
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
                Logger.RegistrarError("Mostrar Productos", ex); // Registra el error usando Logger
                TempData["ErrorMessage"] = "Error al cargar la lista de productos.";
                return RedirectToAction("Index");
            }
        }

        // Método GET para mostrar la vista Create
        [HttpGet]
        public IActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("Mostrar Vista Create", ex); // Registra el error usando Logger
                TempData["ErrorMessage"] = "Error al cargar la vista de creación.";
                return RedirectToAction("Index");
            }
        }

        // Método POST para crear un nuevo producto
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

                    TempData["SuccessMessage"] = $"Producto '{producto.Nombre}' creado exitosamente.";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Logger.RegistrarError("Crear Producto", ex); // Registra el error usando Logger
                    ModelState.AddModelError("", $"Error al crear producto: {ex.Message}");
                }
            }
            return View(producto);
        }

        // Método GET para mostrar la vista Edit
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
                Logger.RegistrarError("Cargar Vista Edit", ex); // Registra el error usando Logger
                TempData["ErrorMessage"] = "Error al cargar la vista.";
                return RedirectToAction("Index");
            }
        }

        // Método POST para editar un producto existente
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

                        TempData["SuccessMessage"] = $"Producto '{producto.Nombre}' actualizado exitosamente.";
                        return RedirectToAction("Index");
                    }
                    TempData["ErrorMessage"] = "Producto no encontrado.";
                }
                catch (Exception ex)
                {
                    Logger.RegistrarError("Editar Producto", ex); // Registra el error usando Logger
                    ModelState.AddModelError("", $"Error al actualizar producto: {ex.Message}");
                }
            }
            return View(producto);
        }

        // Método POST para eliminar un producto
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
                    TempData["SuccessMessage"] = $"Producto '{producto.Nombre}' eliminado exitosamente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "Producto no encontrado.";
                }
            }
            catch (Exception ex)
            {
                Logger.RegistrarError("Eliminar Producto", ex); // Registra el error usando Logger
                TempData["ErrorMessage"] = $"Error al eliminar producto: {ex.Message}";
            }
            return RedirectToAction("Index");
        }
    }
}

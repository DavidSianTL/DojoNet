using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;
using System.Collections.Generic;
using System.Linq;


namespace Proyecto.Controllers
{
    public class ProductoController : Controller
    {
        //* Lista estática para simular una "base de datos"
        private static List<Producto> productos = new List<Producto>
        {
            //new Producto { Id = 1, Nombre = "Producto 1", Precio = 10.99m, Cantidad = 5 },
           // new Producto { Id = 2, Nombre = "Producto 2", Precio = 20.99m, Cantidad = 3 }
        };

        // Mostrar la lista de productos
        public IActionResult Index()
        {
            return View(productos); // Pasa la lista de productos a la vista
        }

        // Vista para crear un nuevo producto
        public IActionResult Create()
        {
            return View();
        }

        // Acción para crear un nuevo producto
        [HttpPost]
        public IActionResult Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                producto.Id = productos.Count + 1; // Asignar un ID único
                productos.Add(producto); // Agregar el producto a la lista
                return RedirectToAction("Index"); // Redirigir al índice de productos
            }

            return View(producto); // Si el modelo no es válido, volver al formulario de creación
        }

        // Vista para editar un producto
        public IActionResult Edit(int id)
        {
            var producto = productos.FirstOrDefault(p => p.Id == id);
            if (producto == null)
            {
                return NotFound(); // Si no se encuentra el producto, devuelve un error 404
            }
            return View(producto); // Pasa el producto a la vista para editar
        }

        // Acción para editar un producto
        [HttpPost]
        public IActionResult Edit(Producto producto)
        {
            if (ModelState.IsValid)
            {
                var productoExistente = productos.FirstOrDefault(p => p.Id == producto.Id);
                if (productoExistente != null)
                {
                    productoExistente.Nombre = producto.Nombre;
                    productoExistente.Precio = producto.Precio;
                    productoExistente.Cantidad = producto.Cantidad;
                    return RedirectToAction("Index"); // Redirigir al índice de productos
                }
                return NotFound(); // Si no se encuentra el producto
            }

            return View(producto); // Si el modelo no es válido, volver al formulario de edición
        }

        // Acción para eliminar un producto
        public IActionResult Delete(int id)
        {
            var producto = productos.FirstOrDefault(p => p.Id == id);
            if (producto == null)
            {
                return NotFound(); // Si no se encuentra el producto, devuelve un error 404
            }

            productos.Remove(producto); // Eliminar el producto de la lista
            return RedirectToAction("Index"); // Redirigir al índice de productos
        }
    }
}


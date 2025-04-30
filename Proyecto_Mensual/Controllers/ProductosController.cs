    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using AppFuturista.Models;
    using AppFuturista.Services;
    using AppFuturista.Filters;

    namespace AppFuturista.Controllers
    {
        [ValidarSesion]
        public class ProductosController : Controller
        {
            private readonly ProductoService _productoService;
            private readonly BitacoraService _bitacoraService;
            
            public ProductosController(ProductoService productoService, BitacoraService bitacoraService)
            {
                _productoService = productoService;
                _bitacoraService = bitacoraService;
            }
            
            [HttpGet]
            public IActionResult Crear()
            {
                return View();
            }
            
            [HttpPost]
            public async Task<IActionResult> Crear(Producto producto)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return View(producto);
                    }
                    
                    await _productoService.CrearAsync(producto);
                    
                    return RedirectToAction("Index", "Panel");
                }
                catch (Exception ex)
                {
                    await _bitacoraService.RegistrarErrorAsync("Productos", "Error al crear producto", ex);
                    ModelState.AddModelError("", "Ha ocurrido un error al crear el producto");
                    return View(producto);
                }
            }
            
            [HttpGet]
            public async Task<IActionResult> Editar(string id)
            {
                try
                {
                    var producto = await _productoService.ObtenerPorIdAsync(id);
                    
                    if (producto == null)
                    {
                        return NotFound();
                    }
                    
                    return View(producto);
                }
                catch (Exception ex)
                {
                    await _bitacoraService.RegistrarErrorAsync("Productos", $"Error al obtener producto para editar (ID: {id})", ex);
                    return RedirectToAction("Error", "Home", new { mensaje = "Error al cargar el producto" });
                }
            }
            
            [HttpPost]
            public async Task<IActionResult> Editar(string id, Producto producto)
            {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return View(producto);
                    }
                    
                    var productoActualizado = await _productoService.ActualizarAsync(id, producto);
                    
                    if (productoActualizado == null)
                    {
                        return NotFound();
                    }
                    
                    return RedirectToAction("Index", "Panel");
                }
                catch (Exception ex)
                {
                    await _bitacoraService.RegistrarErrorAsync("Productos", $"Error al actualizar producto (ID: {id})", ex);
                    ModelState.AddModelError("", "Ha ocurrido un error al actualizar el producto");
                    return View(producto);
                }
            }
            
            [HttpPost]
            public async Task<IActionResult> Eliminar(string id)
            {
                try
                {
                    var resultado = await _productoService.EliminarAsync(id);
                    
                    if (!resultado)
                    {
                        return NotFound();
                    }
                    
                    return RedirectToAction("Index", "Panel");
                }
                catch (Exception ex)
                {
                    await _bitacoraService.RegistrarErrorAsync("Productos", $"Error al eliminar producto (ID: {id})", ex);
                    return RedirectToAction("Error", "Home", new { mensaje = "Error al eliminar el producto" });
                }
            }
            
            // Endpoints AJAX para operaciones modales
            [HttpGet]
            public async Task<IActionResult> ObtenerProducto(string id)
            {
                try
                {
                    var producto = await _productoService.ObtenerPorIdAsync(id);
                    
                    if (producto == null)
                    {
                        return NotFound();
                    }
                    
                    return Json(producto);
                }
                catch (Exception ex)
                {
                    await _bitacoraService.RegistrarErrorAsync("Productos", $"Error al obtener producto por AJAX (ID: {id})", ex);
                    return StatusCode(500, "Error al obtener el producto");
                }
            }
        }
    }

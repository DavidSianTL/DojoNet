using Microsoft.AspNetCore.Mvc;
using CRUD_Evaluacion_Mensual_Abril.Models;


public class ProductoController : Controller
{
    private readonly ProductoService _productoService;
    private readonly BitacoraService _bitacora;

    public ProductoController(ProductoService productoService, BitacoraService bitacora)
    {
        _productoService = productoService;
        _bitacora = bitacora;
    }

    // Acción para mostrar los productos
    public IActionResult Productos()
    {
        var usrNombre = HttpContext.Session.GetString("UsrNombre");
        var NombreCompleto = HttpContext.Session.GetString("NombreCompleto");

        if (usrNombre == null)
        {
            return RedirectToAction("Login", "Login");
        }

        ViewBag.usrNombre = usrNombre;
        ViewBag.NombreCompleto = NombreCompleto;
        _bitacora.RegistrarEvento(HttpContext, usrNombre, $"Consulto productos");
        var productos = _productoService.ObtenerProductos();
        return View("Productos", productos);
    }

    // Acción para crear producto
    [HttpGet]
    public IActionResult Crear()
    {
        var usrNombre = HttpContext.Session.GetString("UsrNombre");
        var NombreCompleto = HttpContext.Session.GetString("NombreCompleto");

        if (usrNombre == null)
        {
            return RedirectToAction("Login", "Login");
        }

        ViewBag.usrNombre = usrNombre;
        ViewBag.NombreCompleto = NombreCompleto;

        return View("CrearProducto");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Crear(ProductoModel producto)
    {
        var usrNombre = HttpContext.Session.GetString("UsrNombre");

        if (ModelState.IsValid)
        {
            _productoService.GuardarProducto(producto);
            _bitacora.RegistrarEvento(HttpContext, usrNombre, $"Creó el producto: {producto.Nombre}");

            TempData["Success"] = "Producto guardado exitosamente.";
            return RedirectToAction("Crear");
        }

        TempData["Error"] = "Corrige los errores del formulario.";
        return View("CrearProducto");
    }

    // Acción para editar producto
    [HttpGet]
    public IActionResult Editar(int index)
    {
        var productos = _productoService.ObtenerProductos();
        var producto = productos.ElementAtOrDefault(index);

        if (producto == null)
        {
            TempData["Error"] = "Producto no encontrado.";
            return RedirectToAction("Productos");
        }

        return PartialView("_EditarProducto", producto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Editar(int index, ProductoModel producto)
    {
        var usrNombre = HttpContext.Session.GetString("UsrNombre");

        if (ModelState.IsValid)
        {
            _productoService.ActualizarProducto(index, producto);
            _bitacora.RegistrarEvento(HttpContext, usrNombre, $"Editó el producto en el índice {index}: {producto.Nombre}");

            TempData["Success"] = "Producto actualizado exitosamente.";
            return RedirectToAction("Productos");
        }

        TempData["Error"] = "Corrige los errores del formulario.";
        return RedirectToAction("Productos");
    }

    // Acción para eliminar producto
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Eliminar(int index)
    {
        var usrNombre = HttpContext.Session.GetString("UsrNombre");

        try
        {
            var productos = _productoService.ObtenerProductos();
            var producto = productos.ElementAtOrDefault(index);

            _productoService.EliminarProducto(index);
            _bitacora.RegistrarEvento(HttpContext, usrNombre, $"Eliminó el producto: {producto?.Nombre ?? "Desconocido"}");

            TempData["Success"] = "Producto eliminado exitosamente.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Error al eliminar el producto: {ex.Message}";
        }

        return RedirectToAction("Productos");
    }
}

using Microsoft.AspNetCore.Mvc;
using CRUD_Evaluacion_Mensual_Abril.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class ProductosApiController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly BitacoraService _bitacora;
    private const string apiUrl = "https://fakestoreapi.com/products";

    public ProductosApiController(HttpClient httpClient, BitacoraService bitacora)
    {
        _httpClient = httpClient;
        _bitacora = bitacora;
    }

    // Acción para mostrar productos desde la API
    public async Task<IActionResult> Index()
    {
        var usrNombre = HttpContext.Session.GetString("UsrNombre");

        if (usrNombre == null)
        {
            return RedirectToAction("Login", "Login"); // Redirige a la página de login si no está autenticado
        }

        var productos = await _httpClient.GetFromJsonAsync<List<ProductoApi>>(apiUrl);

        _bitacora.RegistrarEvento(HttpContext, usrNombre, "Consulto productos desde APIREST");
        return View(productos);
    }

    // Acción para editar un producto
    public async Task<IActionResult> Edit(int id)
    {
        var usrNombre = HttpContext.Session.GetString("UsrNombre");

        if (usrNombre == null)
        {
            return RedirectToAction("Login", "Login"); // Redirige a la página de login si no está autenticado
        }

        var producto = await _httpClient.GetFromJsonAsync<ProductoApi>($"{apiUrl}/{id}");
        if (producto == null)
        {
            TempData["Error"] = "Producto no encontrado.";
            return RedirectToAction("Index");
        }

        return View(producto);
    }

    // Acción para actualizar un producto
    [HttpPost]
    public async Task<IActionResult> Edit(ProductoApi producto)
    {
        var usrNombre = HttpContext.Session.GetString("UsrNombre");

        if (usrNombre == null)
        {
            return RedirectToAction("Login", "Login"); // Redirige a la página de login si no está autenticado
        }

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Datos inválidos. Por favor revisa el formulario.";
            return View(producto);
        }

        try
        {
            var productoJson = new StringContent(
                JsonSerializer.Serialize(producto),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PutAsync($"{apiUrl}/{producto.id}", productoJson);

            if (response.IsSuccessStatusCode)
            {
                _bitacora.RegistrarEvento(HttpContext, usrNombre, $"Edito el producto ID {producto.id}: {producto.title}");
                TempData["Success"] = "Producto editado correctamente.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error al editar el producto.";
                return View(producto);
            }
        }
        catch
        {
            TempData["Error"] = "Error inesperado al editar.";
            return View(producto);
        }
    }

    // Acción para eliminar un producto
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var usrNombre = HttpContext.Session.GetString("UsrNombre");

        if (usrNombre == null)
        {
            return RedirectToAction("Login", "Login"); // Redirige a la página de login si no está autenticado
        }

        var response = await _httpClient.DeleteAsync($"{apiUrl}/{id}");

        if (response.IsSuccessStatusCode)
        {
            _bitacora.RegistrarEvento(HttpContext, usrNombre, $"Elimino el producto con ID {id}");
            TempData["Success"] = "Producto eliminado correctamente.";
        }
        else
        {
            TempData["Error"] = "Error al eliminar el producto.";
        }

        return RedirectToAction("Index");
    }
}

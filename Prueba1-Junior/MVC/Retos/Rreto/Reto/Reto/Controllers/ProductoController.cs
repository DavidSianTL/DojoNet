using System.Linq.Expressions;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Reto.Models;



namespace Reto.Controllers
{
    public class ProductoController : Controller
    {
        private void RegistrarErrores(Exception ex)
        {
            string LogRoute = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

            if (!Directory.Exists(LogRoute))
            {
                Directory.CreateDirectory(LogRoute);
            }

            string FileRoute = Path.Combine(LogRoute, "ErrorLogs");

            string mensaje = $"[{DateTime.Now}] {ex.Message}\n{ex.StackTrace}\n-------------------------\n";
            System.IO.File.AppendAllText(FileRoute, mensaje);

        }

        private readonly string route = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Productos.json");

        private List<Producto> LeerProductos()
        {
            try
            {
                if (!System.IO.File.Exists(route))
                {
                    return new List<Producto>();
                }

                string contenido = System.IO.File.ReadAllText(route);

                if (string.IsNullOrWhiteSpace(contenido))
                {
                    return new List<Producto>();
                }

                List<Producto> ListaProductos = JsonSerializer.Deserialize<List<Producto>>(contenido) ?? new List<Producto>();

                return ListaProductos;
            }
            catch (Exception ex) {
                RegistrarErrores(ex);

                return new List<Producto>();
            }
        }

        private string GuardarProductos(List<Producto> productos)
        {
            try{

                string contenido = JsonSerializer.Serialize<List<Producto>>(productos, new JsonSerializerOptions { WriteIndented = true });

                System.IO.File.WriteAllText(route, contenido);
                return "Producto guardado con exito";

            }catch(Exception ex){

                RegistrarErrores(ex);

                return "Error al guardar el producto!";
            }


        }


        [HttpGet]
        public IActionResult Index()
        {
            //si el usuario es valido estará guardado en la sesion
            string usuario = HttpContext.Session.GetString("Usuario");

            //si es null o vacío entonces no lo hemos guardado; si no lo hemos guardado entonces no era valido/ o no inició sesión
            if (string.IsNullOrEmpty(usuario)) return RedirectToAction("Login", "Login"); // y lo redirigimos a la vista login
            List<Producto> Productos = LeerProductos();
            //si está en la sesión lo redirigimos a la vista Producto
            return View(Productos);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            string usuario = HttpContext.Session.GetString("Usuario");

            if (string.IsNullOrEmpty(usuario))
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Producto producto)
        {
            try {
                string usuario = HttpContext.Session.GetString("Usuario");

                if (string.IsNullOrEmpty(usuario)){
                    return RedirectToAction("Login", "Login");
                }


                List<Producto> productos = LeerProductos();

                //generamos un id
                int idNuevo = productos.Any() ? productos.Max(p => p.Id) + 1 : 1;
                //añadimos ese id
                producto.Id = idNuevo;

                productos.Add(producto);

                GuardarProductos(productos);
            }catch(Exception ex) {
                RegistrarErrores(ex);

            }
                return RedirectToAction("Index", "Producto");
            
        }





        [HttpGet]
        public IActionResult Editar(int id)
        {
            try
            {
                string usuario = HttpContext.Session.GetString("Usuario");

                if (string.IsNullOrEmpty(usuario))
                {
                    return RedirectToAction("Login", "Login");
                }


                List<Producto> productos = LeerProductos();

                Producto producto = productos.FirstOrDefault(p => p.Id == id);

                if (producto == null)
                {
                    return NotFound();
                }

                return View(producto);
            }catch (Exception ex){
                RegistrarErrores(ex);
                return NotFound();
                
            }
        }

        [HttpPost]
        public IActionResult Editar(Producto producto)
        {
            try{

                string usuario = HttpContext.Session.GetString("Usuario");

                if (string.IsNullOrEmpty(usuario))
                {
                    return RedirectToAction("Login", "Login");
                }

                if (ModelState.IsValid)
                {
                    var productos = LeerProductos();

                    var index = productos.FindIndex(p => p.Id == producto.Id);

                    if (index != -1)
                    {
                        productos[index] = producto;

                        GuardarProductos(productos);

                        return RedirectToAction("Index", "Producto");
                    }

                    return NotFound();
                }

                }catch (Exception ex){

                RegistrarErrores(ex);

                }

            return View(producto);
        }


        [HttpPost]
        public IActionResult Eliminar(int id)
        {

            try
            {

                string usuario = HttpContext.Session.GetString("Usuario");
                if (string.IsNullOrEmpty(usuario))
                {
                    return RedirectToAction("Login", "Login");
                }



                var productos = LeerProductos();
                var producto = productos.FirstOrDefault(p => p.Id == id);

                if (producto != null)
                {
                    productos.Remove(producto);
                    GuardarProductos(productos);
                }

            }
            catch (Exception ex){
                RegistrarErrores(ex);
            }

                return RedirectToAction("Index", "Producto");
        }

    }
}

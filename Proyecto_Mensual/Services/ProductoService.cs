using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppFuturista.Models;

namespace AppFuturista.Services
{
    public class ProductoService
    {
        private readonly ArchivoJsonService _archivoJsonService;
        private readonly BitacoraService _bitacoraService;
        private const string NombreArchivo = "productos.json";
        
        public ProductoService(ArchivoJsonService archivoJsonService, BitacoraService bitacoraService)
        {
            _archivoJsonService = archivoJsonService;
            _bitacoraService = bitacoraService;
            InicializarDatosAsync().Wait();
        }
        
        private async Task InicializarDatosAsync()
        {
            var productos = await _archivoJsonService.LeerAsync<Producto>(NombreArchivo);
            
            if (productos.Count == 0)
            {
                // Agregar datos de ejemplo si no existen productos
                productos.Add(new Producto
                {
                    Id = Guid.NewGuid().ToString(),
                    Nombre = "Laptop Cuántica",
                    Descripcion = "Laptop de alta velocidad con procesamiento cuántico y 32GB RAM",
                    Categoria = "Electrónica",
                    Precio = 2499.99m,
                    ImagenUrl = "/img/laptop.png",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                });
                
                productos.Add(new Producto
                {
                    Id = Guid.NewGuid().ToString(),
                    Nombre = "Silla Ergonómica Inteligente",
                    Descripcion = "Silla de oficina con ajuste automático y soporte lumbar adaptativo",
                    Categoria = "Hogar",
                    Precio = 349.99m,
                    ImagenUrl = "/img/silla.png",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                });
                
                await _archivoJsonService.EscribirAsync(NombreArchivo, productos);
                await _bitacoraService.RegistrarEventoAsync("Sistema", "Datos de productos inicializados con ejemplos");
            }
        }
        
        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            await _bitacoraService.RegistrarEventoAsync("Productos", "Consulta de todos los productos");
            return await _archivoJsonService.LeerAsync<Producto>(NombreArchivo);
        }
        
        public async Task<Producto> ObtenerPorIdAsync(string id)
        {
            var productos = await _archivoJsonService.LeerAsync<Producto>(NombreArchivo);
            var producto = productos.FirstOrDefault(p => p.Id == id);
            
            if (producto != null)
            {
                await _bitacoraService.RegistrarEventoAsync("Productos", $"Consulta del producto: {producto.Nombre} (ID: {id})");
            }
            else
            {
                await _bitacoraService.RegistrarEventoAsync("Productos", $"Intento de consulta de producto inexistente (ID: {id})");
            }
            
            return producto;
        }
        
        public async Task<Producto> CrearAsync(Producto producto)
        {
            var productos = await _archivoJsonService.LeerAsync<Producto>(NombreArchivo);
            
            producto.Id = Guid.NewGuid().ToString();
            producto.FechaCreacion = DateTime.Now;
            producto.FechaActualizacion = DateTime.Now;
            
            if (string.IsNullOrEmpty(producto.ImagenUrl))
            {
                producto.ImagenUrl = "/img/producto-default.png";
            }
            
            productos.Add(producto);
            await _archivoJsonService.EscribirAsync(NombreArchivo, productos);
            
            await _bitacoraService.RegistrarEventoAsync("Productos", $"Producto creado: {producto.Nombre} (ID: {producto.Id})");
            
            return producto;
        }
        
        public async Task<Producto> ActualizarAsync(string id, Producto productoActualizado)
        {
            var productos = await _archivoJsonService.LeerAsync<Producto>(NombreArchivo);
            var productoExistente = productos.FirstOrDefault(p => p.Id == id);
            
            if (productoExistente == null)
            {
                await _bitacoraService.RegistrarEventoAsync("Productos", $"Intento de actualización de producto inexistente (ID: {id})");
                return null;
            }
            
            productoExistente.Nombre = productoActualizado.Nombre;
            productoExistente.Descripcion = productoActualizado.Descripcion;
            productoExistente.Categoria = productoActualizado.Categoria;
            productoExistente.Precio = productoActualizado.Precio;
            
            if (!string.IsNullOrEmpty(productoActualizado.ImagenUrl))
            {
                productoExistente.ImagenUrl = productoActualizado.ImagenUrl;
            }
            
            productoExistente.FechaActualizacion = DateTime.Now;
            
            await _archivoJsonService.EscribirAsync(NombreArchivo, productos);
            
            await _bitacoraService.RegistrarEventoAsync("Productos", $"Producto actualizado: {productoExistente.Nombre} (ID: {id})");
            
            return productoExistente;
        }
        
        public async Task<bool> EliminarAsync(string id)
        {
            var productos = await _archivoJsonService.LeerAsync<Producto>(NombreArchivo);
            var producto = productos.FirstOrDefault(p => p.Id == id);
            
            if (producto == null)
            {
                await _bitacoraService.RegistrarEventoAsync("Productos", $"Intento de eliminación de producto inexistente (ID: {id})");
                return false;
            }
            
            productos.Remove(producto);
            await _archivoJsonService.EscribirAsync(NombreArchivo, productos);
            
            await _bitacoraService.RegistrarEventoAsync("Productos", $"Producto eliminado: {producto.Nombre} (ID: {id})");
            
            return true;
        }
    }
}

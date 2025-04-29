using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using wsServicioAuth.Models;

namespace wsServicioAuth
{
    /// <summary>
    /// Descripción breve de AuthService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class AuthService : System.Web.Services.WebService
    {

        // Simulamos una base de datos de usuarios
        private static readonly List<User> users = new List<User>
        {
            new User { Id = 1, Username = "Daniel Roblero", Email = "droblero@digitalgeko.com", Password = "Password123!" },
            new User { Id = 2, Username = "Erick", Email = "ebarrera@digitalgeko.com", Password = "Erick456@" },
            new User { Id = 3, Username = "DiegoC", Email = "dcatalan@digitalgeko.com", Password = "Diego789$" },
            new User { Id = 4, Username = "Anika Escoto", Email = "aescoto@digitalgeko.com", Password = "Anika321!" },
            new User { Id = 5, Username = "ccaal", Email = "ccaal@digitalgeko.com", Password = "Ccaal654#" },
            new User { Id = 6, Username = "JoPalvarado", Email = "jperalta@digitalgeko.com", Password = "Jopalvarado987@" },
            new User { Id = 7, Username = "Pablo Torres", Email = "ptorres@digitalgeko.com", Password = "Pablo123#" }
        };


        [WebMethod(Description = "Valida las credenciales de un usuario.")]
        public LoginResponse ValidateLogin(string username, string password)
        {
            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                return new LoginResponse
                {
                    Success = true,
                    Message = $"Bienvenido, {user.Username}!",
                    FullName = user.Username,
                    Email = user.Email
                };
            }
            else
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Usuario o contraseña incorrectos.",
                    FullName = null,
                    Email = null
                };
            }
        }


        // Simulación de productos
        private static readonly List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop Dell XPS 13", Price = 1200.00m },
            new Product { Id = 2, Name = "Smartphone Samsung S24", Price = 999.99m },
            new Product { Id = 3, Name = "Audífonos Sony WH-1000XM5", Price = 349.99m },
            new Product { Id = 4, Name = "Monitor LG UltraFine 27\"", Price = 599.99m },
            new Product { Id = 5, Name = "Teclado Mecánico Logitech G915", Price = 249.99m },
            new Product { Id = 6, Name = "Mouse Logitech MX Master 3", Price = 99.99m },
            new Product { Id = 7, Name = "Tablet iPad Pro 12.9\"", Price = 1299.99m },
            new Product { Id = 8, Name = "Cámara Canon EOS R6", Price = 2499.99m },
            new Product { Id = 9, Name = "Consola PlayStation 5", Price = 499.99m },
            new Product { Id = 10, Name = "Xbox Series X", Price = 499.99m },
            new Product { Id = 11, Name = "Silla Gamer Secretlab", Price = 429.99m },
            new Product { Id = 12, Name = "SSD Samsung 980 PRO 1TB", Price = 189.99m },
            new Product { Id = 13, Name = "Procesador AMD Ryzen 9 7950X", Price = 699.99m },
            new Product { Id = 14, Name = "Motherboard ASUS ROG Strix X670E", Price = 499.99m },
            new Product { Id = 15, Name = "Memoria RAM Corsair 32GB DDR5", Price = 199.99m },
            new Product { Id = 16, Name = "Tarjeta de Video NVIDIA RTX 4090", Price = 1599.99m },
            new Product { Id = 17, Name = "Fuente de Poder Corsair 850W", Price = 149.99m },
            new Product { Id = 18, Name = "Gabinete NZXT H710", Price = 169.99m },
            new Product { Id = 19, Name = "Router WiFi 6 TP-Link", Price = 129.99m },
            new Product { Id = 20, Name = "Kindle Paperwhite 11th Gen", Price = 139.99m },
            new Product { Id = 21, Name = "Smartwatch Apple Watch Series 9", Price = 399.99m },
            new Product { Id = 22, Name = "Drone DJI Mini 4 Pro", Price = 999.99m },
            new Product { Id = 23, Name = "Proyector Epson Home Cinema", Price = 799.99m },
            new Product { Id = 24, Name = "Smart TV Samsung QLED 55\"", Price = 899.99m },
            new Product { Id = 25, Name = "Sistema de Sonido Bose", Price = 499.99m },
            new Product { Id = 26, Name = "Tablet Samsung Galaxy Tab S9", Price = 899.99m },
            new Product { Id = 27, Name = "Reproductor de Streaming Roku Ultra", Price = 99.99m },
            new Product { Id = 28, Name = "Aire acondicionado inteligente LG", Price = 699.99m },
            new Product { Id = 29, Name = "Aspiradora Inteligente Roomba", Price = 399.99m },
            new Product { Id = 30, Name = "Smartphone Google Pixel 8", Price = 899.99m }
        };

        // Método para obtener lista de productos
        [WebMethod(Description = "Obtiene todos los productos disponibles.")]
        public List<Product> GetProducts()
        {
            return products;
        }


        // Método para obtener un producto por su ID
        [WebMethod(Description = "Obtiene un producto específico por su ID.")]
        public Product GetProductById(int id)
        {
            // Busca el producto en la lista que tenga el mismo ID
            var product = products.FirstOrDefault(p => p.Id == id);

            // Retorna el producto encontrado (o null si no existe)
            return product;
        }


    }
}

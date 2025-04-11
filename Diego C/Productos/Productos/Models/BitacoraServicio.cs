using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Productos.Models
{
    public class BitacoraServicio
    {
        private readonly string _ruta = Path.Combine(Directory.GetCurrentDirectory(), "bitacora.txt");

        public void RegistrarEvento(HttpContext context, string usuario, string mensaje)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString();
            var navegador = context.Request.Headers["User-Agent"].ToString();

            using (var writer = new StreamWriter(_ruta, true))
            {
                writer.WriteLine("---------------------------------------------------");
                writer.WriteLine($"Fecha: {DateTime.Now}");
                writer.WriteLine($"Usuario: {usuario}");
                writer.WriteLine($"Evento: {mensaje}");
                writer.WriteLine($"IP: {ip}");
                writer.WriteLine($"Navegador: {navegador}");
                writer.WriteLine();
            }
        }
    }
}
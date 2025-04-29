using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace CRUD_Evaluacion_Mensual_Abril.Models
{
    public class BitacoraService
    {
        private readonly string _ruta = Path.Combine(Directory.GetCurrentDirectory(), "bitacora.txt");

        public void RegistrarEvento(HttpContext context, string usuario, string mensaje)
        {
           
            using (var writer = new StreamWriter(_ruta, true))
            {
                writer.WriteLine("---------------------------------------------------");
                writer.WriteLine($"Fecha: {DateTime.Now}");
                writer.WriteLine($"Usuario: {usuario}");
                writer.WriteLine($"Evento: {mensaje}");
                writer.WriteLine();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExamDaniel.bitacora
{
    public static class BitacoraManager
    {
        private static readonly string _rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "bitacora.txt");

        
        public static void RegistrarEvento(string tipoEvento, string descripcion)
        {
           
            descripcion = RemoverTildes(descripcion);

            string evento = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {tipoEvento} | {descripcion}";

           
            File.AppendAllText(_rutaArchivo, evento + Environment.NewLine);
        }

        
        public static List<string> LeerEventos()
        {
            List<string> eventos = new List<string>();

            if (File.Exists(_rutaArchivo))
            {
                var lineas = File.ReadAllLines(_rutaArchivo);
             
                eventos.AddRange(lineas.Reverse());
            }

            return eventos;
        }

        private static string RemoverTildes(string texto)
        {
            var sb = new StringBuilder(texto);
            sb.Replace("á", "a");
            sb.Replace("é", "e");
            sb.Replace("í", "i");
            sb.Replace("ó", "o");
            sb.Replace("ú", "u");
            sb.Replace("Á", "A");
            sb.Replace("É", "E");
            sb.Replace("Í", "I");
            sb.Replace("Ó", "O");
            sb.Replace("Ú", "U");
            sb.Replace("ü", "u");
            sb.Replace("Ü", "U");
            return sb.ToString();
        }
    }
}


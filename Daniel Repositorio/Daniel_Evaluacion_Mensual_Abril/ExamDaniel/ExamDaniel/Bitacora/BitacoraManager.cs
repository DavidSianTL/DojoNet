using System;
using System.Collections.Generic;
using System.IO;

namespace ExamDaniel.bitacora
{
    public static class BitacoraManager
    {
        private static readonly string _rutaArchivo = Path.Combine(Directory.GetCurrentDirectory(), "bitacora.txt");

        // Método para registrar un evento
        public static void RegistrarEvento(string tipoEvento, string descripcion)
        {
            string evento = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {tipoEvento} | {descripcion}";
            File.AppendAllText(_rutaArchivo, evento + Environment.NewLine);
        }

        // Método para leer los eventos de la bitácora
        public static List<string> LeerEventos()
        {
            List<string> eventos = new List<string>();

            if (File.Exists(_rutaArchivo))
            {
                var lineas = File.ReadAllLines(_rutaArchivo);
                foreach (var linea in lineas)
                {
                    eventos.Add(linea);
                }
            }

            return eventos;
        }
    }
}

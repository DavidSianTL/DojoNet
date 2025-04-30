using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Final_Evaluacion_Mensual_Abril.Models;

namespace Proyecto1.Services
{
    public class BitacoraService
    {
        private readonly string _logFilePath;
        private readonly string _fallbackLogsDir;

        public BitacoraService(IWebHostEnvironment env)
        {
            var logsDir = Path.Combine(env.ContentRootPath, "wwwroot", "logs");
            Directory.CreateDirectory(logsDir);
            _logFilePath = Path.Combine(logsDir, "bitacora.json");

            _fallbackLogsDir = Path.Combine(logsDir, "fallback-logs");
            Directory.CreateDirectory(_fallbackLogsDir);

            if (!File.Exists(_logFilePath))
            {
                File.WriteAllText(_logFilePath, "[]");
            }
        }

        public void RegistrarEvento(LogEntry entry)
        {
            try
            {
                var logs = ObtenerLogs();
                logs.Add(entry);

                var options = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(_logFilePath, JsonSerializer.Serialize(logs, options));
            }
            catch (Exception ex)
            {
                RegistrarEnFallback(entry, ex);
            }
        }

        private void RegistrarEnFallback(LogEntry entry, Exception ex)
        {
            try
            {
                string fallbackPath = Path.Combine(_fallbackLogsDir, $"bitacora-fallback-{DateTime.Now:yyyyMMdd}.txt");
                string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | " +
                                  $"Usuario: {entry.Usuario ?? "N/A"} | " +
                                  $"Acción: {entry.Accion} | " +
                                  $"Controlador: {entry.Controlador} | " +
                                  $"Error: {ex.Message}\n";

                File.AppendAllText(fallbackPath, logMessage);
            }
            catch (Exception fallbackEx)
            {
                Console.WriteLine($"FALLBACK LOG FAILED: {fallbackEx.Message}");
            }
        }

        public List<LogEntry> ObtenerLogs()
        {
            try
            {
                var json = File.ReadAllText(_logFilePath);
                return JsonSerializer.Deserialize<List<LogEntry>>(json) ?? new List<LogEntry>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al leer bitácora: {ex.Message}");
                return new List<LogEntry>();
            }
        }

        public List<LogEntry> FiltrarLogs(DateTime? desde = null, DateTime? hasta = null, string usuario = null)
        {
            var logs = ObtenerLogs();

            if (desde.HasValue)
                logs = logs.Where(l => l.Fecha >= desde.Value).ToList();

            if (hasta.HasValue)
                logs = logs.Where(l => l.Fecha <= hasta.Value).ToList();

            if (!string.IsNullOrEmpty(usuario))
                logs = logs.Where(l => l.Usuario == usuario).ToList();

            return logs.OrderByDescending(l => l.Fecha).ToList();
        }
    }
}
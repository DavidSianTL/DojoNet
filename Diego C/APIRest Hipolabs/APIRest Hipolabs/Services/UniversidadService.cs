using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using APIRest_Hipolabs.Models;

namespace APIRest_Hipolabs.Services
{
    public class UniversidadService
    {
        private readonly HttpClient _httpClient;

        public UniversidadService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<UniversidadModel>> ObtenerUniversidadesPorPais(string pais)
        {
            // Codificar el nombre del país para evitar problemas con espacios y caracteres especiales
            var url = $"http://universities.hipolabs.com/search?country={Uri.EscapeDataString(pais)}";

            try
            {
                using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(10)); // Timeout de 10 segundos
                var respuesta = await _httpClient.GetAsync(url, cts.Token);

                if (respuesta.IsSuccessStatusCode)
                {
                    var contenido = await respuesta.Content.ReadAsStringAsync();
                    // Deserializamos la respuesta JSON
                    return JsonSerializer.Deserialize<List<UniversidadModel>>(contenido, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
                else
                {
                    // Si la API devuelve un código de error, puedes manejarlo o devolver un valor vacío
                    return new List<UniversidadModel>();
                }
            }
            catch (Exception ex)
            {
                // Logueamos el error (podrías hacer algo más elaborado)
                Console.WriteLine($"Error al consultar universidades: {ex.Message}");
                return new List<UniversidadModel>();
            }
        }
    }
}

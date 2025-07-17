using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProyectoDojoGeko.Services
{

    // Interfaz para el servicio de paises
    public interface ICountryService
    {
        Task<List<string>> ObtenerPaises();
    }

    public class CountryService : ICountryService
    {
        // Inyectamos el HttpClient (para hacer peticiones HTTP)
        private readonly HttpClient _httpClient;

        // Constructor
        public CountryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Obtenemos los paises de la API
        public async Task<List<string>> ObtenerPaises()
        {
            // Obtenemos la respuesta de la API
            var response = await _httpClient.GetAsync("https://countriesnow.space/api/v0.1/countries");

            // Validamos que la respuesta sea exitosa
            response.EnsureSuccessStatusCode();

            // Obtenemos el contenido de la respuesta
            var json = await response.Content.ReadAsStringAsync();

            // Creamos una lista para almacenar los paises
            var countries = new List<string>();

            // Parseamos el JSON
            using (JsonDocument doc = JsonDocument.Parse(json))
            {
                // Guardamos el elemento raíz del JSON (root sirve para acceder a los elementos)
                var root = doc.RootElement;

                // Verificamos si el JSON tiene la propiedad "data"
                if (root.TryGetProperty("data", out var dataArray))
                {
                    // Iteramos sobre cada elemento del array "data"
                    foreach (var element in dataArray.EnumerateArray())
                    {
                        // Verificamos si el elemento tiene la propiedad "country"
                        if (element.TryGetProperty("country", out var countryProp))
                        {
                            // Obtenemos el nombre del pais y lo agregamos a la lista
                            var countryName = countryProp.GetString();

                            // Validamos que el nombre del pais no sea nulo o vacío
                            if (!string.IsNullOrWhiteSpace(countryName))
                                countries.Add(countryName);
                        }
                    }
                }
            }

            // Ordenamos los paises
            countries.Sort();

            // Retornamos la lista de paises
            return countries;
        }
    }
}
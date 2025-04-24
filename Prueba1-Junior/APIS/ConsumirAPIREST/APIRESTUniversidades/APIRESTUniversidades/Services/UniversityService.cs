using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using APIRESTUniversidades.Models;

namespace APIRESTUniversidades.Services
{
    public class UniversityService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://universities.hipolabs.com";

        public UniversityService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<University>> UniversidadPorPais(string nombrePais)
        {
            var url = $"{_baseUrl}/search?country={nombrePais}";
            var universities = await _httpClient.GetFromJsonAsync<List<University>>(url);

            return universities ?? new List<University>();

        }

        public async Task<List<University>>UniversidadPorNombre(string nombre)
        {
            var url = $"{_baseUrl}/search?name={nombre}";
            var universidad = await _httpClient.GetFromJsonAsync<List<University>>(url);

            return universidad ?? new List<University>();
        }




    }
}

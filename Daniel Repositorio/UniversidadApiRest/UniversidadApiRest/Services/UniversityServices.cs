using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using UniversidadApiRest.Models;

namespace UniversidadApiRest.Services
{
    public class UniversityService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "http://universities.hipolabs.com/search";

        public UniversityService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<University>> GetUniversitiesByCountryAsync(string country)
        {
            // Si el país está vacío, devolvemos una lista vacía directamente
            if (string.IsNullOrWhiteSpace(country))
                return new List<University>();

            var url = $"{_baseUrl}?country={country}";
            var response = await _httpClient.GetFromJsonAsync<List<University>>(url);
            return response ?? new List<University>();
        }
    }
}

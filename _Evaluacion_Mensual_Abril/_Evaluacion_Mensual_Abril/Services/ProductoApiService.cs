using System.Net.Http;
using System.Text.Json;
using _Evaluacion_Mensual_Abril.Models;

namespace _Evaluacion_Mensual_Abril.Services
{
    public class ProductoApiService
    {
        private readonly HttpClient _httpClient;

        public ProductoApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ProductoApi>> ObtenerProductosAsync()
        {
            var response = await _httpClient.GetAsync("https://fakestoreapi.com/products");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ProductoApi>>(json);
        }
        public async Task<ProductoApi> ObtenerProductoPorId(int id)
        {
            var response = await _httpClient.GetAsync($"https://fakestoreapi.com/products/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ProductoApi>(json);
        }


    }

}

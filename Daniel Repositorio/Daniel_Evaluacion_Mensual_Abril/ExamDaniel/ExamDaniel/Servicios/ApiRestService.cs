using System.Net.Http.Json;
using ExamDaniel.Models;

namespace ExamDaniel.Servicios
{
    public class ApiRestService
    {
        private readonly HttpClient _httpClient;

        public ApiRestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://fakestoreapi.com/");
        }

        public async Task<List<ProductoApiRest>> ObtenerProductosAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ProductoApiRest>>("products")
                   ?? new List<ProductoApiRest>();
        }

        public async Task<ProductoApiRest> CrearProductoAsync(ProductoApiRest producto)
        {
            var response = await _httpClient.PostAsJsonAsync("products", producto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProductoApiRest>();
        }

        public async Task<ProductoApiRest> EditarProductoAsync(int id, ProductoApiRest producto)
        {
            var response = await _httpClient.PutAsJsonAsync($"products/{id}", producto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProductoApiRest>();
        }

        public async Task<ProductoApiRest> EliminarProductoAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"products/{id}");
            response.EnsureSuccessStatusCode();
            // La API devuelve el objeto eliminado
            return await response.Content.ReadFromJsonAsync<ProductoApiRest>();
        }
    }
}

using Final_Evaluacion_Mensual_Abril.Models;
using System.Net.Http.Json;

namespace Final_Evaluacion_Mensual_Abril.Services
{
    public class FakeStoreService
    {
        private readonly HttpClient _httpClient;

        public FakeStoreService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("FakeStoreAPI");
        }

       
        public async Task<List<ProductoApi>> GetProductsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ProductoApi>>("products");
        }

        public async Task<ProductoApi> GetProductByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<ProductoApi>($"products/{id}");
        }

       
        public async Task<List<UsuarioApi>> GetUsersAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<UsuarioApi>>("users");
        }

        

        // Métodos para carritos
        public async Task<List<CarritoApi>> GetCartsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CarritoApi>>("carts");
        }

       

        
        public async Task<List<string>> GetCategoriesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<string>>("products/categories");
        }

        public async Task<List<ProductoApi>> GetProductsByCategoryAsync(string category)
        {
            return await _httpClient.GetFromJsonAsync<List<ProductoApi>>($"products/category/{category}");
        }

       
        public async Task<ProductoApi> AddProductAsync(ProductoApi product)
        {
            var response = await _httpClient.PostAsJsonAsync("products", product);
            return await response.Content.ReadFromJsonAsync<ProductoApi>();
        }

        public async Task<UsuarioApi> AddUserAsync(UsuarioApi user)
        {
            var response = await _httpClient.PostAsJsonAsync("users", user);
            return await response.Content.ReadFromJsonAsync<UsuarioApi>();
        }
    }
}
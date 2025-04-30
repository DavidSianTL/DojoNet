using Examen_mes_abril.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Examen_mes_abril.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://fakestoreapi.com/products";
        public ProductService()
        {
            _httpClient = new HttpClient();
        }

        // Lista para obtener todos los productos de la Api
        public async Task<List<ProductModel>> GetAllProductsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<ProductModel>>(_baseUrl);
            return response ?? new List<ProductModel>();
        }

        // Obtiene un producto por medio del id
        public async Task<ProductModel> GetProductByIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<ProductModel>($"{_baseUrl}/{id}");
        }

        // Simula la creación de un producto
        public async Task<HttpResponseMessage> CreateProductAsync(ProductModel product)
        {
            return await _httpClient.PostAsJsonAsync(_baseUrl, product);
        }

        // Simula editar un producto
        public async Task<HttpResponseMessage> UpdateProductAsync(string id, ProductModel product)
        {
            return await _httpClient.PutAsJsonAsync($"{_baseUrl}/{id}", product);
        }

        // Simula eliminar un producto
        public async Task<HttpResponseMessage> DeleteProductAsync(string id)
        {
            return await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
        }
    }

}

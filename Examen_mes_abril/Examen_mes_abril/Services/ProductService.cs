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
        public async Task<List<ProductModel>> GetAllProductsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<ProductModel>>(_baseUrl);
            return response ?? new List<ProductModel>();
        }
        public async Task<ProductModel> GetProductByIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<ProductModel>($"{_baseUrl}/{id}");
        }
        public async Task<HttpResponseMessage> CreateDeviceAsync(ProductModel device)
        {
            return await _httpClient.PostAsJsonAsync(_baseUrl, device);
        }

        public async Task<HttpResponseMessage> UpdateProductAsync(string id, ProductModel device)
        {
            return await _httpClient.PutAsJsonAsync($"{_baseUrl}/{id}", device);
        }

        public async Task<HttpResponseMessage> DeleteProductAsync(string id)
        {
            return await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
        }
    }

}

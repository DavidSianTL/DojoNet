using System.Text.Json;
using AspNetCoreGeneratedDocument;
using ExamenUno.Models;
using Microsoft.AspNetCore.Authentication;

namespace ExamenUno.Services
{
	public interface IFakeStoreAPIService
	{
        Task<FakeProduct> CreateProdAsync(FakeProduct fakeproduct);
        public Task<List<FakeProduct>> ShowProdAsync();
		public Task<FakeProduct> EditProdAsync(int id, FakeProduct editedProduct);

		public Task<bool> DeleteProdAsync(int id);

    }
	public class FakeStoreAPIService : IFakeStoreAPIService
	{
		private readonly HttpClient _httpClient = new HttpClient();

		private readonly string baseUrl = "https://fakestoreapi.com";

		public FakeStoreAPIService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}


		public async Task<List<FakeProduct>> ShowProdAsync()
		{
			var url = $"{baseUrl}/products";
			var fakeProducts = await _httpClient.GetFromJsonAsync<List<FakeProduct>>(url);
			
			return fakeProducts;
		}

		public async Task<FakeProduct> CreateProdAsync(FakeProduct fakeproduct)
		{
			var url =$"{baseUrl}/products";
			var response = await _httpClient.PostAsJsonAsync<FakeProduct>(url, fakeproduct);

			return await response.Content.ReadFromJsonAsync<FakeProduct>();
        }

        public async Task<FakeProduct> EditProdAsync(int id, FakeProduct editedProduct)
        {
            var url = $"{baseUrl}/products/{id}";
            var response = await _httpClient.PutAsJsonAsync(url, editedProduct);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<FakeProduct>();
        }

        public async Task<bool> DeleteProdAsync(int id)
        {
            var url = $"{baseUrl}/products/{id}";
            var response = await _httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }


    }
}

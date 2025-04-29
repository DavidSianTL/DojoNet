using System.Text.Json;
using AspNetCoreGeneratedDocument;
using ExamenUno.Models;
using Microsoft.AspNetCore.Authentication;

namespace ExamenUno.Services
{
	public interface IFakeStoreAPIService
	{

		public Task<List<FakeProduct>> ShowProdAsync();

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


		
	}
}

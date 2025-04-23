using ConsumiendoAPIREST.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace ConsumiendoAPIREST.Services
{
    public class DeviceService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://api.restful-api.dev/objects";

        public DeviceService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Device>> GetAllDevicesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Device>>(_baseUrl);
            return response ?? new List<Device>();
        }

        public async Task<Device> GetDeviceByIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<Device>($"{_baseUrl}/{id}");
        }

        public async Task<HttpResponseMessage> CreateDeviceAsync(Device device)
        {
            return await _httpClient.PostAsJsonAsync(_baseUrl, device);
        }

        public async Task<HttpResponseMessage> UpdateDeviceAsync(string id, Device device)
        {
            return await _httpClient.PutAsJsonAsync($"{_baseUrl}/{id}", device);
        }

        public async Task<HttpResponseMessage> DeleteDeviceAsync(string id)
        {
            return await _httpClient.DeleteAsync($"{_baseUrl}/{id}");
        }
    }
}

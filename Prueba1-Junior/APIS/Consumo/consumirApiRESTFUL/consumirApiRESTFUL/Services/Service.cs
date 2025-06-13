using consumirApiRESTFUL.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace consumirApiRESTFUL.Services
{
    public class Service
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://restful-api.dev/";

        public DeviceService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Device>> GetAllDeviceAsync()
        {

            var response = await _httpClient.GetFromJsonAsync<List<Device>>(_baseUrl);
            return response ?? new List<Device>();

        }

    }


}

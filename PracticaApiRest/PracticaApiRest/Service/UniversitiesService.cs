using PracticaApiRest.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace PracticaApiRest.Service
{
    public class UniversitiesService
    {
        private readonly HttpClient _httpClient;
        private readonly string _Url = "http://universities.hipolabs.com";


        public UniversitiesService()
        {
            _httpClient = new HttpClient();
        }

        //public async Task<List<UniversitiesModel>> GetAllUniversitiesAsync()
        //{
        //    var respuesta = await _httpClient.GetFromJsonAsync<List<UniversitiesModel>>(_Url);
        //    return respuesta ?? new List<UniversitiesModel>();
        ////}

        public async Task<UniversitiesModel?> GetUniversityByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            name = Uri.EscapeDataString(name); 
            var url = $"{_Url}/search?name={name}";

            var lista = await _httpClient.GetFromJsonAsync<List<UniversitiesModel>>(url);
            return lista?.FirstOrDefault();
        }

        public async Task<HttpResponseMessage> CreateUniversityAsync(UniversitiesModel universities)
        {
            return await _httpClient.PostAsJsonAsync(_Url, universities);

        }
        public async Task<HttpResponseMessage> UpdateUniversityAsync(string name, UniversitiesModel universities)
        {
            return await _httpClient.PutAsJsonAsync($"{_Url}/{name}", universities);
        }
        public async Task<HttpResponseMessage> DeleteUniversityAsync(string name)
        {
            return await _httpClient.DeleteAsync($"{_Url}/{name}");
        }
    }
}

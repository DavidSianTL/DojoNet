using EjemploConsumirApiRest.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EjemploConsumirApiRest.Services
{
    public class DeviceService
    {

        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://api.restful-api.dev/objects"; // Cambia esto por la URL de tu API

        public DeviceService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<ServiceViewModel>> GetAllDevicesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<ServiceViewModel>>(_baseUrl);

            /*if (response == null)
            {
                //throw new Exception("No se pudo obtener la lista de dispositivos.");
            

                // Los '??' sirven para proteger el código de un posible null
                // Si la respuesta es null, se devuelve una lista vacía
                return new List<ServiceViewModel>();
            }
            else
            {
                return response;
            }*/

            return response ?? new List<ServiceViewModel>();


        }





    }
}

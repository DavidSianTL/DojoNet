using CountryInfoService;
using EjemploConsumoServicioSOAP.CountryInfoService;
using System.Threading.Tasks;

namespace EjemploConsumoServicioSOAP.Services
{
    public class CountryService
    {
        private readonly CountryInfoServiceSoapTypeClient _client;

        public CountryService()
        {
            _client = new CountryInfoServiceSoapTypeClient(CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);
        }

        public async Task<string> GetFlagUrlAsync(string countryCode)
        {
            var response = await _client.CountryFlagAsync(countryCode);
            return response.Body.CountryFlagResult;
        }
    }
}


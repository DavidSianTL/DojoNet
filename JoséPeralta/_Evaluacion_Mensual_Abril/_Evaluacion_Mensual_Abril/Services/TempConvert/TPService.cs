using wsTempConvert;

namespace _Evaluacion_Mensual_Abril.Services.TempConvert
{
    public interface ITPService
    {
        Task<string> CelsiusToFahrenheitAsync(string celsius);
        Task<string> FahrenheitToCelsiusAsync(string fahrenheit);
    }

    public class TPService : ITPService
    {
        private readonly TempConvertSoapClient _client;

        public TPService()
        {
            _client = new TempConvertSoapClient(TempConvertSoapClient.EndpointConfiguration.TempConvertSoap);
        }

        public async Task<string> CelsiusToFahrenheitAsync(string celsius)
        {
            return await _client.CelsiusToFahrenheitAsync(celsius);
        }

        public async Task<string> FahrenheitToCelsiusAsync(string fahrenheit)
        {
            return await _client.FahrenheitToCelsiusAsync(fahrenheit);
        }
    }
}

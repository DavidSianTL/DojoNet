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

            // Verifica si el valor de Celsius es nulo o vacío
            if (string.IsNullOrEmpty(celsius))
            {
                throw new ArgumentException("Celsius value cannot be null or empty.", nameof(celsius));
            }

            // Llamada al servicio SOAP
            var resultado = await _client.CelsiusToFahrenheitAsync(celsius);

            // Verifica si el resultado es nulo o vacío
            if (string.IsNullOrEmpty(resultado))
            {
                throw new Exception("Error converting Celsius to Fahrenheit.");
            }

            // Cierra la conexión después de usarlo
            await _client.CloseAsync();

            // Retorna el resultado
            return resultado;

        }

        public async Task<string> FahrenheitToCelsiusAsync(string fahrenheit)
        {

            // Verifica si el valor de Fahrenheit es nulo o vacío
            if (string.IsNullOrEmpty(fahrenheit))
            {
                throw new ArgumentException("Celsius value cannot be null or empty.", nameof(fahrenheit));
            }

            // Llamada al servicio SOAP
            var resultado = await _client.FahrenheitToCelsiusAsync(fahrenheit);

            // Verifica si el resultado es nulo o vacío
            if (string.IsNullOrEmpty(resultado))
            {
                throw new Exception("Error converting Celsius to Fahrenheit.");
            }

            // Cierra la conexión después de usarlo
            await _client.CloseAsync();

            // Retorna el resultado
            return resultado;


        }
    }




}

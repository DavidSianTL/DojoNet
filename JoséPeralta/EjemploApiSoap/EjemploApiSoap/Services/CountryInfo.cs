using CountryInfoService;

namespace EjemploApiSoap.Services
{
    public interface ICountryInfo
    {
        Task<string> CountryInfoService(string country); 
    }

    public class CountryInfo : ICountryInfo
    {
        public async Task<string> CountryInfoService(string country)
        {
            try
            {
                // Validación de entrada
                if (string.IsNullOrEmpty(country))
                {
                    File.WriteAllText("log.txt", "El país no puede ser nulo.");
                    throw new ArgumentNullException(nameof(country), "El país no puede ser nulo.");
                }

                // Conexión a servicio SOAP
                var cliente = new CountryInfoServiceSoapTypeClient(CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);

                // Llamada al servicio SOAP
                var resultado = await cliente.CapitalCityAsync(country);

                // Verificamos que el resultado no sea nulo
                if (resultado == null)
                {
                    File.WriteAllText("log.txt", "No se pudo obtener la capital de Guatemala.");
                    throw new Exception("No se pudo obtener la capital de Guatemala.");
                }

                // Cierre de la conexión
                await cliente.CloseAsync();

                // Retorno del resultado
                return resultado.Body.CapitalCityResult;
            }
            catch (Exception e)
            {
                // Manejo de excepciones
                System.IO.File.WriteAllText("log.txt", e.ToString());
                return "Error al consultar la Capital: " + e.Message;
            }
        }
    }

}
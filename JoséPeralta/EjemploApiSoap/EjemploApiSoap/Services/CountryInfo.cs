using CountryInfoService;

namespace EjemploApiSoap.Services
{
    public interface ICountryInfo
    {
        Task<string> CountryInfoService(string country);
        Task<string> ConsultarCapitalPorCodigo(string codigoPais);
        Task<List<tLanguage>> ListaDeIdiomasPorNombre();
        Task<List<tLanguage>> ListaDeIdiomasPorCodigo();
        Task<string> ConsultarIdioma(string idioma);

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

        public async Task<string> ConsultarCapitalPorCodigo(string codigoPais)
        {
            try
            {
                //conexion al servicio soap 
                var cliente = new CountryInfoServiceSoapTypeClient(CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);

                var resultado = await cliente.CapitalCityAsync(codigoPais);

                await cliente.CloseAsync();

                return resultado.Body.CapitalCityResult;
            }
            catch (Exception e)
            {
                System.IO.File.WriteAllText("log.txt", e.ToString());
                return "Error al consultar la Capital: " + e.Message;
            }
        }
        public async Task<List<tLanguage>> ListaDeIdiomasPorNombre()
        {

            try
            {
                // Conexión al servicio SOAP
                var cliente = new CountryInfoServiceSoapTypeClient(CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);

                // Llamada al método correcto del servicio SOAP
                var resultado = await cliente.ListOfLanguagesByNameAsync();

                // Verificar que el resultado no sea nulo
                if (resultado?.Body?.ListOfLanguagesByNameResult != null)
                {
                    // Convertir el resultado a una lista de tLanguage
                    return resultado.Body.ListOfLanguagesByNameResult.ToList();

                }

                // Cierre de la conexión
                await cliente.CloseAsync();

                // Si el resultado es nulo, devolver una lista vacía
                return new List<tLanguage>(); // Devolver una lista vacía en caso de error

            }
            catch (Exception e)
            {
                System.IO.File.WriteAllText("log.txt", e.ToString());
                return new List<tLanguage>(); // Devolver una lista vacía en caso de error
            }


        }

        public async Task<List<tLanguage>> ListaDeIdiomasPorCodigo()
        {
            try
            {
                // Conexión al servicio SOAP
                var cliente = new CountryInfoServiceSoapTypeClient(CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);

                // Llamada al método correcto del servicio SOAP
                var resultado = await cliente.ListOfLanguagesByCodeAsync();

                // Verificar que el resultado no sea nulo
                if (resultado?.Body?.ListOfLanguagesByCodeResult != null)
                {
                    // Convertir el resultado a una lista de tLanguage
                    return resultado.Body.ListOfLanguagesByCodeResult.ToList();

                }

                // Cierre de la conexión
                await cliente.CloseAsync();

                // Si el resultado es nulo, devolver una lista vacía
                return new List<tLanguage>(); // Devolver una lista vacía en caso de error

            }
            catch (Exception e)
            {
                // Manejo de excepciones
                System.IO.File.WriteAllText("log.txt", e.ToString());
                return new List<tLanguage>(); // Devolver una lista vacía en caso de error

            }
        }

        public async Task<string> ConsultarIdioma(string idioma)
        {

            try
            {
                // Conexión al servicio SOAP
                var cliente = new CountryInfoServiceSoapTypeClient(CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);

                // Llamada al método correcto del servicio SOAP
                var resultado = await cliente.LanguageNameAsync(idioma);

                // Cierre de la conexión
                await cliente.CloseAsync();

                // Si el resultado es nulo, devolver el mensaje de error
                if (resultado?.Body?.LanguageNameResult != null)
                {
                    return resultado.Body.LanguageNameResult;
                }
                else
                {
                    return "No se encontró el idioma.";
                }


            }
            catch (Exception e)
            {
                // Manejo de excepciones
                System.IO.File.WriteAllText("log.txt", e.ToString());
                return "Error al consultar el idioma: " + e.Message;

            }


        }








    }

}
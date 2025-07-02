
using ServiceReference1;

namespace NuevoEjemploConsumoApiSoap.Services.NumberService
{

    public interface IConsumoDeMetodos
    {
        Task<string> ConsumoDeMetodosAsync(int number);
    }

    public class ConsumoDeMetodos : IConsumoDeMetodos
    {

        public async Task<string> ConsumoDeMetodosAsync(int number)
        {

            try
            {
                // Conexión al servicio SOAP
                var cliente = new NumberConversionSoapTypeClient(NumberConversionSoapTypeClient.EndpointConfiguration.NumberConversionSoap12);

                // Llamada al método del servicio SOAP
                var resultado = await cliente.NumberToDollarsAsync(number);

                // Cerramos la conexión 
                await cliente.CloseAsync();

                // Devolvemos el resultado
                return resultado.Body.NumberToDollarsResult;

            }
            catch (Exception e)
            {
                // Manejo de excepciones
                return $"Error: {e.Message}";
            }


        }




    }
}

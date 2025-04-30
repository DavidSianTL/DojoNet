using wsNumberConversion;

namespace ConsumoSoap2.Servicio
{
    public interface IConsumoDeMetodo
    {
        Task<string> ConvertNumerosALetras();
    }
    public class ConsumoDeMetodo: IConsumoDeMetodo
    {
        public async Task<string> ConvertNumerosALetras()
        {
            try
            {
                int Number = 1; // Cambia este valor por el número que deseas convertir
                // Crear una instancia del cliente del servicio web
                var client = new NumberConversionSoapTypeClient(NumberConversionSoapTypeClient.EndpointConfiguration.NumberConversionSoap);
                // Llamar al método del servicio web
                var result = await client.NumberToWordsAsync(Number);
                //cerramos la conexión
                await client.CloseAsync();
                // Procesar la respuesta
                return result.Body.NumberToWordsResult;
              
            }
            catch (Exception ex)
            {
                throw new Exception("Error al consumir el servicio: " + ex.Message);
            }
         
        }
    }
}

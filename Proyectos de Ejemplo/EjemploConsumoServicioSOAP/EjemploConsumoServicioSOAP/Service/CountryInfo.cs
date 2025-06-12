using CountryInfoService;

namespace EjemploConsumoServicioSOAP.Service
{
    public interface ICountryInfo
    {
        Task<string> ConsultarCapitalPorCodigo(string codigoPais);
        Task<string> CountryInfoService();
    }

    public class CountryInfo: ICountryInfo
    {
        
        public async Task<string> CountryInfoService()
        {
            try
            {
                //conexion al servicio soap 
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );


                var resultado = await cliente.CapitalCityAsync("GT");

                if (resultado is null)
                {
                    return $"No se encintraron resultados";
                }

                await cliente.CloseAsync();

                return resultado.Body.CapitalCityResult;
            }
            catch (Exception ex)
            {

                return $"Error al consultar la capital: {ex.Message}";
            }
        }


        public async Task<string> ConsultarCapitalPorCodigo(string codigoPais)
        {
            try
            {
                //conexion al servicio soap 
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                var resultado = await cliente.CapitalCityAsync(codigoPais);
               

                await cliente.CloseAsync();

                return resultado.Body.CapitalCityResult;

                
            }
            catch (Exception ex)
            {
                return $"Error al consultar la capital: {ex.Message}";
            }
        }


    }
}

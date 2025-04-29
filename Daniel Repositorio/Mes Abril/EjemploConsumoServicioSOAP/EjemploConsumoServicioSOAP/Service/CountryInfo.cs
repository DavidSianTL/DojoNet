using CountryInfoService;

namespace EjemploConsumoServicioSOAP.Service
{
    public interface ICountryInfo
    {
        Task<string> ConsultarCapitalPorCodigo(string codigoPais);
        Task<string> CountryInfoService();

        Task<List<tLanguage>> ObtenerIdiomas();
        Task<List<tLanguage>> ObtenerIdiomasPorCodigo();
        Task<string> ObtenerNombreIdiomaPorCodigo(string codigoIdioma);
        Task<string> ObtenerCodigoIdiomaPorNombre(string nombreIdioma);


    }

    public class CountryInfo : ICountryInfo
    {
        public async Task<string> CountryInfoService()
        {
            try
            {
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                var resultado = await cliente.CapitalCityAsync("GT");

                if (resultado is null)
                {
                    return $"No se encontraron resultados";
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

        // Devuelve una variedad de idiomas ordenados por nombre
        public async Task<List<tLanguage>> ObtenerIdiomas()
        {
            try
            {
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                var resultado = await cliente.ListOfLanguagesByNameAsync();

                await cliente.CloseAsync();

                return resultado.Body.ListOfLanguagesByNameResult.ToList();
            }
            catch (Exception)
            {
                return new List<tLanguage>(); 
            }
        }
        //Devuelve una variedad de idiomas ordenados por código
        public async Task<List<tLanguage>> ObtenerIdiomasPorCodigo()
        {
            try
            {
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                var resultado = await cliente.ListOfLanguagesByCodeAsync();

                await cliente.CloseAsync();

                return resultado.Body.ListOfLanguagesByCodeResult.ToList();
            }
            catch (Exception)
            {
                return new List<tLanguage>(); 
            }
        }

        //Encuentre un nombre de idioma basado en el código de idioma ISO aprobado
        public async Task<string> ObtenerNombreIdiomaPorCodigo(string codigoIdioma)
        {
            try
            {
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                var response = await cliente.LanguageNameAsync(codigoIdioma);

                await cliente.CloseAsync();

                return response.Body.LanguageNameResult;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        //Encuentre un código ISO de idioma basado en el nombre del idioma pasado
        public async Task<string> ObtenerCodigoIdiomaPorNombre(string nombreIdioma)
        {
            try
            {
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                var response = await cliente.LanguageISOCodeAsync(nombreIdioma);

                await cliente.CloseAsync();

                return response.Body.LanguageISOCodeResult;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

    }


}

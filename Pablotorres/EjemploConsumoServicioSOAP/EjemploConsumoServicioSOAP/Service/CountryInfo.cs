using CountryInfoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace EjemploConsumoServicioSOAP.Service
{
    public interface ICountryInfo
    {
        Task<string> ConsultarCapitalPorCodigo(string codigoPais);
        Task<string> CountryInfoService();
        Task<List<tLanguage>> ListarIdiomasPorNombre();
        Task<List<tLanguage>> ListarIdiomasPorCodigo();
        Task<string> ObtenerNombreIdiomaPorCodigo(string codigoIdioma);
        Task<string> ObtenerCodigoISOPorNombreIdioma(string nombreIdioma);
        Task<string> ObtenerBanderaPorCodigo(string codigoPais);
    }

    public class CountryInfo : ICountryInfo
    {
        public async Task<string> CountryInfoService()
        {
            try
            {
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);

                var resultado = await cliente.CapitalCityAsync("GT");

                if (resultado is null)
                {
                    return "No se encontraron resultados";
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
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);

                var resultado = await cliente.CapitalCityAsync(codigoPais);

                await cliente.CloseAsync();

                return resultado.Body.CapitalCityResult;
            }
            catch (Exception ex)
            {
                return $"Error al consultar la capital: {ex.Message}";
            }
        }

        public async Task<List<tLanguage>> ListarIdiomasPorNombre()
        {
            try
            {
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);

                var response = await cliente.ListOfLanguagesByNameAsync();

                if (response?.Body?.ListOfLanguagesByNameResult == null)
                {
                    return new List<tLanguage>();
                }

                await cliente.CloseAsync();

                return response.Body.ListOfLanguagesByNameResult.ToList();
            }
            catch (Exception ex)
            {

                throw new Exception($"Error al obtener el listado de idiomas: {ex.Message}");


            }
        }
        public async Task<List<tLanguage>> ListarIdiomasPorCodigo()
        {
            try
            {
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                var response = await cliente.ListOfLanguagesByCodeAsync();

                if (response?.Body?.ListOfLanguagesByCodeResult == null)
                    return new List<tLanguage>();

                await cliente.CloseAsync();
                return response.Body.ListOfLanguagesByCodeResult.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al listar idiomas por código: {ex.Message}");
            }
        }
        public async Task<string> ObtenerNombreIdiomaPorCodigo(string codigoIdioma)
        {
            try
            {
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);

                var response = await cliente.LanguageNameAsync(codigoIdioma);

                if (string.IsNullOrEmpty(response?.Body?.LanguageNameResult))
                    return "Código de idioma no encontrado";

                await cliente.CloseAsync();
                return response.Body.LanguageNameResult;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener nombre de idioma: {ex.Message}");
            }
        }
        public async Task<string> ObtenerCodigoISOPorNombreIdioma(string nombreIdioma)
        {
            try
            {
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);

                var response = await cliente.LanguageISOCodeAsync(nombreIdioma);

                if (string.IsNullOrEmpty(response?.Body?.LanguageISOCodeResult))
                    return "Nombre de idioma no encontrado";

                await cliente.CloseAsync();
                return response.Body.LanguageISOCodeResult;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener código ISO: {ex.Message}");
            }
        }
        public async Task<string> ObtenerBanderaPorCodigo(string codigoPais)
        {
            try
            {
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);

                var response = await cliente.CountryFlagAsync(codigoPais);

                if (string.IsNullOrEmpty(response?.Body?.CountryFlagResult))
                    return null;

                return response.Body.CountryFlagResult;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener bandera: {ex.Message}");
            }
            

        }

    }
}
using CountryInfoService;
using System.ServiceModel;

namespace EjemploConsumoServicioSOAP.Service
{
    public class CountryInfo : ICountryInfo
    {
        private CountryInfoServiceSoapTypeClient CrearCliente()
        {
            return new CountryInfoServiceSoapTypeClient(
                CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);
        }

        public async Task<string> ConsultarCapitalPorCodigo(string codigoPais)
        {
            try
            {
                using var cliente = CrearCliente();
                var resultado = await cliente.CapitalCityAsync(codigoPais);
                return resultado.Body.CapitalCityResult;
            }
            catch (Exception ex)
            {
                return $"Error al consultar la capital: {ex.Message}";
            }
        }

        public async Task<string> ObtenerNombrePais(string codigoPais)
        {
            try
            {
                using var cliente = CrearCliente();
                var resultado = await cliente.CountryNameAsync(codigoPais);
                return resultado.Body.CountryNameResult;
            }
            catch (Exception ex)
            {
                return $"Error al consultar el nombre del país: {ex.Message}";
            }
        }

        public async Task<string> ObtenerMonedaPais(string codigoPais)
        {
            try
            {
                using var cliente = CrearCliente();
                var resultado = await cliente.CountryCurrencyAsync(codigoPais);
                var moneda = resultado.Body.CountryCurrencyResult;
                return $"{moneda.sISOCode} - {moneda.sName}";
            }
            catch (Exception ex)
            {
                return $"Error al consultar la moneda: {ex.Message}";
            }
        }

        public async Task<string> ObtenerBanderaPais(string codigoPais)
        {
            try
            {
                using var cliente = CrearCliente();
                var resultado = await cliente.CountryFlagAsync(codigoPais);
                return resultado.Body.CountryFlagResult;
            }
            catch (Exception ex)
            {
                return $"Error al consultar la bandera: {ex.Message}";
            }
        }

        public async Task<string> ListarContinentesPorNombre()
        {
            try
            {
                using var cliente = CrearCliente();
                var resultado = await cliente.ListOfContinentsByNameAsync();
                return FormatearLista(resultado.Body.ListOfContinentsByNameResult, c => $"{c.sCode} - {c.sName}");
            }
            catch (Exception ex)
            {
                return $"Error al listar continentes: {ex.Message}";
            }
        }

        public async Task<string> ListarContinentesPorCodigo()
        {
            try
            {
                using var cliente = CrearCliente();
                var resultado = await cliente.ListOfContinentsByCodeAsync();
                return FormatearLista(resultado.Body.ListOfContinentsByCodeResult, c => $"{c.sCode} - {c.sName}");
            }
            catch (Exception ex)
            {
                return $"Error al listar continentes: {ex.Message}";
            }
        }

        public async Task<string> ListarMonedasPorNombre()
        {
            try
            {
                using var cliente = CrearCliente();
                var resultado = await cliente.ListOfCurrenciesByNameAsync();
                return FormatearLista(resultado.Body.ListOfCurrenciesByNameResult, c => $"{c.sISOCode} - {c.sName}");
            }
            catch (Exception ex)
            {
                return $"Error al listar monedas: {ex.Message}";
            }
        }

        public async Task<string> ListarMonedasPorCodigo()
        {
            try
            {
                using var cliente = CrearCliente();
                var resultado = await cliente.ListOfCurrenciesByCodeAsync();
                return FormatearLista(resultado.Body.ListOfCurrenciesByCodeResult, c => $"{c.sISOCode} - {c.sName}");
            }
            catch (Exception ex)
            {
                return $"Error al listar monedas: {ex.Message}";
            }
        }

        public async Task<string> ListarPaisesPorCodigo()
        {
            try
            {
                using var cliente = CrearCliente();
                var resultado = await cliente.ListOfCountryNamesByCodeAsync();
                return FormatearLista(resultado.Body.ListOfCountryNamesByCodeResult, p => $"{p.sISOCode} - {p.sName}");
            }
            catch (Exception ex)
            {
                return $"Error al listar países: {ex.Message}";
            }
        }

        public async Task<string> ListarPaisesPorNombre()
        {
            try
            {
                using var cliente = CrearCliente();
                var resultado = await cliente.ListOfCountryNamesByNameAsync();
                return FormatearLista(resultado.Body.ListOfCountryNamesByNameResult, p => $"{p.sISOCode} - {p.sName}");
            }
            catch (Exception ex)
            {
                return $"Error al listar países: {ex.Message}";
            }
        }

        public async Task<string> ListarPaisesAgrupadosPorContinente()
        {
            try
            {
                using var cliente = CrearCliente();
                var resultado = await cliente.ListOfCountryNamesGroupedByContinentAsync();
                
                var lista = new List<string>();
                foreach (var continente in resultado.Body.ListOfCountryNamesGroupedByContinentResult)
                {
                    lista.Add($"Continente: {continente.Continent}");
                    lista.AddRange(continente.CountryCodeAndNames.Select(p => $"  {p.sISOCode} - {p.sName}"));
                    lista.Add("");
                }
                
                return string.Join(Environment.NewLine, lista);
            }
            catch (Exception ex)
            {
                return $"Error al listar países agrupados: {ex.Message}";
            }
        }

        public async Task<string> ObtenerNombreIdioma(string codigoIdioma)
        {
            try
            {
                using var cliente = CrearCliente();
                var resultado = await cliente.LanguageNameAsync(codigoIdioma);
                return resultado.Body.LanguageNameResult;
            }
            catch (Exception ex)
            {
                return $"Error al consultar el idioma: {ex.Message}";
            }
        }

        public async Task<string> ObtenerIdiomasPais(string codigoPais)
        {
        try
         {
        using var cliente = CrearCliente();
        // Versión corregida, asumiendo que ListOfLanguagesByCodeAsync no requiere parámetros
        var resultado = await cliente.ListOfLanguagesByCodeAsync();
        return FormatearLista(resultado.Body.ListOfLanguagesByCodeResult, l => $"{l.sISOCode} - {l.sName}");
        }
        catch (Exception ex)
        {
        return $"Error al consultar idiomas: {ex.Message}";
        }
        }
        

        private string FormatearLista<T>(IEnumerable<T> items, Func<T, string> formateador)
        {
            return string.Join(Environment.NewLine, items.Select(formateador));
        }
    }
}   
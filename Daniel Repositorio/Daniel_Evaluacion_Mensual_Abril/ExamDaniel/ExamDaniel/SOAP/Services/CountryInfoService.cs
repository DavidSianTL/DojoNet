using ExamDaniel.SOAP.Models;
using CountryServiceReference;

namespace ExamDaniel.Servicios
{
    public class CountryInfoService
    {
        private readonly CountryInfoServiceSoapTypeClient _cliente;

        public CountryInfoService()
        {
            _cliente = new CountryInfoServiceSoapTypeClient(CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);
        }

        public async Task<tCountryCodeAndName[]> ObtenerPaisesPorNombreAsync()
        {
            var resultado = await _cliente.ListOfCountryNamesByNameAsync();
            return resultado.Body.ListOfCountryNamesByNameResult;
        }

        public async Task<tCountryCodeAndName[]> ObtenerPaisesPorCodigoAsync()
        {
            var resultado = await _cliente.ListOfCountryNamesByCodeAsync();
            return resultado.Body.ListOfCountryNamesByCodeResult;
        }

        public async Task<CountryInfo> ObtenerInformacionCompletaAsync(string codigo)
        {
            // Obtener la información completa del país usando el código
            var resultado = await _cliente.FullCountryInfoAsync(codigo);

            
            var countryInfo = new CountryInfo
            {
                sCountryName = resultado.Body.FullCountryInfoResult.sName, // Nombre del país
                sISOCode = resultado.Body.FullCountryInfoResult.sISOCode, // Código ISO
                sCapitalCity = resultado.Body.FullCountryInfoResult.sCapitalCity, // Capital
                sRegion = resultado.Body.FullCountryInfoResult.sContinentCode, // Continente
                sSubRegion = resultado.Body.FullCountryInfoResult.sPhoneCode, // Código telefónico
                sCurrency = resultado.Body.FullCountryInfoResult.sCurrencyISOCode, // Código de moneda
                sArea = resultado.Body.FullCountryInfoResult.sCountryFlag // Link 
            };

            return countryInfo;
        }
    }
}




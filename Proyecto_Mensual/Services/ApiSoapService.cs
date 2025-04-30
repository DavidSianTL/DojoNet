using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using AppFuturista.Models;
using Microsoft.Extensions.Logging;

namespace AppFuturista.Services
{
    public class ApiSoapService
    {
        private readonly HttpClient _httpClient;
        private readonly BitacoraService _bitacoraService;
        private readonly ILogger<ApiSoapService> _logger;
        private readonly string _baseUrl;
        
        public ApiSoapService(
            HttpClient httpClient, 
            BitacoraService bitacoraService, 
            ILogger<ApiSoapService> logger,
            ArchivoJsonService archivoJsonService)
        {
            _httpClient = httpClient;
            _bitacoraService = bitacoraService;
            _logger = logger;
            
            var configuracion = archivoJsonService.LeerConfiguracionAsync<ConfiguracionApp>("configuracion.json").Result;
            _baseUrl = configuracion.UrlApiSoap;
        }
        
        public async Task<string> ObtenerNombrePaisPorCodigoAsync(string codigoPais)
        {
            try
            {
                await _bitacoraService.RegistrarEventoAsync("API SOAP", $"Consultando información del país con código {codigoPais}");
                
                // Crear la solicitud SOAP
                var soapEnvelope = new StringBuilder();
                soapEnvelope.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                soapEnvelope.Append("<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
                soapEnvelope.Append("<soap:Body>");
                soapEnvelope.Append("<CountryName xmlns=\"http://www.oorsprong.org/websamples.countryinfo\">");
                soapEnvelope.Append($"<sCountryISOCode>{codigoPais}</sCountryISOCode>");
                soapEnvelope.Append("</CountryName>");
                soapEnvelope.Append("</soap:Body>");
                soapEnvelope.Append("</soap:Envelope>");
                
                var content = new StringContent(soapEnvelope.ToString(), Encoding.UTF8, "text/xml");
                content.Headers.Add("SOAPAction", "http://www.oorsprong.org/websamples.countryinfo/CountryName");
                
                var response = await _httpClient.PostAsync(_baseUrl, content);
                response.EnsureSuccessStatusCode();
                
                var responseString = await response.Content.ReadAsStringAsync();
                
                // Procesar la respuesta XML
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseString);
                
                // Definir los namespaces para XPath
                var nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
                nsManager.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                nsManager.AddNamespace("m", "http://www.oorsprong.org/websamples.countryinfo");
                
                // Extraer el nombre del país
                var nombrePais = xmlDoc.SelectSingleNode("//m:CountryNameResult", nsManager)?.InnerText;
                
                await _bitacoraService.RegistrarEventoAsync("API SOAP", $"Obtenido nombre de país: {nombrePais} para código {codigoPais}");
                
                return nombrePais ?? "No encontrado";
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarErrorAsync("API SOAP", $"Error al obtener información del país con código {codigoPais}", ex);
                _logger.LogError(ex, "Error al consultar la API SOAP");
                throw;
            }
        }
        
        public async Task<string> ObtenerCapitalPorCodigoAsync(string codigoPais)
        {
            try
            {
                await _bitacoraService.RegistrarEventoAsync("API SOAP", $"Consultando capital del país con código {codigoPais}");
                
                // Crear la solicitud SOAP
                var soapEnvelope = new StringBuilder();
                soapEnvelope.Append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
                soapEnvelope.Append("<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\">");
                soapEnvelope.Append("<soap:Body>");
                soapEnvelope.Append("<CapitalCity xmlns=\"http://www.oorsprong.org/websamples.countryinfo\">");
                soapEnvelope.Append($"<sCountryISOCode>{codigoPais}</sCountryISOCode>");
                soapEnvelope.Append("</CapitalCity>");
                soapEnvelope.Append("</soap:Body>");
                soapEnvelope.Append("</soap:Envelope>");
                
                var content = new StringContent(soapEnvelope.ToString(), Encoding.UTF8, "text/xml");
                content.Headers.Add("SOAPAction", "http://www.oorsprong.org/websamples.countryinfo/CapitalCity");
                
                var response = await _httpClient.PostAsync(_baseUrl, content);
                response.EnsureSuccessStatusCode();
                
                var responseString = await response.Content.ReadAsStringAsync();
                
                // Procesar la respuesta XML
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(responseString);
                
                // Definir los namespaces para XPath
                var nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
                nsManager.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                nsManager.AddNamespace("m", "http://www.oorsprong.org/websamples.countryinfo");
                
                // Extraer la capital
                var capital = xmlDoc.SelectSingleNode("//m:CapitalCityResult", nsManager)?.InnerText;
                
                await _bitacoraService.RegistrarEventoAsync("API SOAP", $"Obtenida capital: {capital} para país con código {codigoPais}");
                
                return capital ?? "No encontrado";
            }
            catch (Exception ex)
            {
                await _bitacoraService.RegistrarErrorAsync("API SOAP", $"Error al obtener capital del país con código {codigoPais}", ex);
                _logger.LogError(ex, "Error al consultar la API SOAP");
                throw;
            }
        }
    }
}

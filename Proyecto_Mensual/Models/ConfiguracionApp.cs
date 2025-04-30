namespace AppFuturista.Models
{
    public class ConfiguracionApp
    {
        public string Idioma { get; set; } = "es-ES";
        public string TemaColor { get; set; } = "azul";
        public string UrlApiRest { get; set; } = "https://fakestoreapi.com";
        public string UrlApiSoap { get; set; } = "http://webservices.oorsprong.org/websamples.countryinfo/CountryInfoService.wso";
        public string RutaBitacora { get; set; } = "Logs/bitacora.txt";
        public string RutaErrores { get; set; } = "Logs/errores.txt";
    }
}

using CountryInfoService;
namespace ConsumoApiSOAP.Services
{

	public interface ICountryInfoService
	{
		public Task<string> paisPorCodigoAsync(string codigoISO);

        public Task<string> capitalDePaisPorCodigoAsync(string codigoISO);
        Task<string> CodigoPorPaisAsync(string nombrePais);
    }

    public class ServicioCountryInfo : ICountryInfoService
	{

		private CountryInfoServiceSoapTypeClient crearCliente()
		{
			return new CountryInfoServiceSoapTypeClient(
				CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
			);
			
		}
        

		public async Task<string> paisPorCodigoAsync(string codigoISO)
		{
			var cliente = crearCliente();

			var content = await cliente.CountryNameAsync(codigoISO);

			await cliente.CloseAsync();

			return content.Body.CountryNameResult;
        }

        public async Task<string> capitalDePaisPorCodigoAsync(string codigoISO)
		{
			var cliente = crearCliente();

            var content = await cliente.CapitalCityAsync(codigoISO);

			await cliente.CloseAsync();


			return content.Body.CapitalCityResult;
        }

		public async Task<string> CodigoPorPaisAsync(string nombrePais)
		{
			var cliente = crearCliente();

			var content = await cliente.CountryISOCodeAsync(nombrePais);

			await cliente.CloseAsync();

			return content.Body.CountryISOCodeResult;

        }

    }
}

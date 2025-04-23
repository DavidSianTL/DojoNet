using CountryInfoService;

namespace ConsumoServiciosSoap.Servicios
{

    public interface ICountryInfoService
    {
        Task<string> ConsultarCapitalPorCodigo(string codigoPais);
        Task<string> CapitalCityAsync();
        Task<List<string>> ListOfContinentsByNameAsync();
        Task<List<string>> ListOfContinentsByCode();
    }

    public class ServicioCountryInfo: ICountryInfoService
    {
        //Devuelve el nombre de la ciudad capital para el código de país pasado
        public async Task<string> CapitalCityAsync()
        {
            try
            {
                //Conexion a servicio
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                var resultado = await cliente.CapitalCityAsync("GT");

                await cliente.CloseAsync();

                return resultado.Body.CapitalCityResult; // Retornar el resultado
            }
            catch (Exception ex)
            {
                return $"Error al consultar la capital: {ex.Message}";
            }
        }


        //Devuelve una lista de continentes ordenados por nombre.
        public async Task<List<string>> ListOfContinentsByNameAsync()
        {
            var continentes = new List<string>();
            try
            {
                //Conexion a servicio
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                //Llamar al metodo
                var resultado = await cliente.ListOfContinentsByNameAsync();

                await cliente.CloseAsync();

                //Retornar el resultado
                foreach (var item in resultado.Body.ListOfContinentsByNameResult)
                {
                    //Agregar a la lista los parametros devueltos por el servicio
                    continentes.Add($"{item.sCode} - {item.sName}");
                }
            }
            catch (Exception ex)
            {
                continentes.Add($"Error al obtener continentes: {ex.Message}");
            }

            return continentes;
        }


        //Devuelve una lista de continentes ordenados por código.
        public async Task<List<string>> ListOfContinentsByCode()
        {
            var continentes = new List<string>();
            try
            {
                //Conexion a servicio
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                //Llamar al metodo
                var resultado = await cliente.ListOfContinentsByCodeAsync();

                await cliente.CloseAsync();

                //Retornar el resultado
                foreach (var item in resultado.Body.ListOfContinentsByCodeResult)
                {
                    //Agregar a la lista los parametros devueltos por el servicio
                    continentes.Add($"{item.sCode} - {item.sName}");
                }
            }
            catch (Exception ex)
            {
                continentes.Add($"Error al obtener continentes: {ex.Message}");
            }

            return continentes;
        }


        //Devuelve una lista de monedas ordenadas por código.
        public async Task<List<string>> ListOfCurrenciesByCode()
        {
            var continentes = new List<string>();
            try
            {
                //Conexion a servicio
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                //Llamar al metodo
                var resultado = await cliente.ListOfCurrenciesByCodeAsync();

                await cliente.CloseAsync();

                //Retornar el resultado
                foreach (var item in resultado.Body.ListOfCurrenciesByCodeResult)
                {
                    //Agregar a la lista los parametros devueltos por el servicio
                    continentes.Add($"{item.sISOCode} - {item.sName}");
                }
            }
            catch (Exception ex)
            {
                continentes.Add($"Error al obtener continentes: {ex.Message}");
            }

            return continentes;
        }


        //Devuelve una lista de monedas ordenadas por nombre.
        public async Task<string> CurrencyName(string sCurrencyISOCode)
        {
            try
            {
                //Conexion a servicio
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                var resultado = await cliente.CurrencyNameAsync("AED");

                await cliente.CloseAsync();

                //Retornar el resultado
                return resultado.Body.CurrencyNameResult;
            }
            catch (Exception ex)
            {
                return $"Error al consultar la capital: {ex.Message}";
            }

        }


        //Devuelve una lista de todos los condados almacenados ordenados por código ISO
        public async Task<List<string>> ListOfCountryNamesByCode()
        {
            var continentes = new List<string>();
            try
            {
                //Conexion a servicio
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                //Llamar al metodo
                var resultado = await cliente.ListOfCountryNamesByCodeAsync();

                await cliente.CloseAsync();

                //Retornar el resultado
                foreach (var item in resultado.Body.ListOfCountryNamesByCodeResult)
                {
                    //Agregar a la lista los parametros devueltos por el servicio
                    continentes.Add($"{item.sISOCode} - {item.sName}");
                }
            }
            catch (Exception ex)
            {
                continentes.Add($"Error al obtener continentes: {ex.Message}");
            }

            return continentes;
        }


        //Devuelve una lista de todos los condados almacenados ordenados por nombre de país
        public async Task<List<string>> ListOfCountryNamesByName()
        {
            var continentes = new List<string>();
            try
            {
                //Conexion a servicio
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                //Llamar al metodo
                var resultado = await cliente.ListOfCountryNamesByNameAsync();

                await cliente.CloseAsync();

                //Retornar el resultado
                foreach (var item in resultado.Body.ListOfCountryNamesByNameResult)
                {
                    //Agregar a la lista los parametros devueltos por el servicio
                    continentes.Add($"{item.sISOCode} - {item.sName}");
                }
            }
            catch (Exception ex)
            {
                continentes.Add($"Error al obtener continentes: {ex.Message}");
            }

            return continentes;
        }


        //Devuelve una lista de todos los condados almacenados agrupados por continente
        public async Task<List<string>> ListOfCountryNamesGroupedByContinent()
        {
            var continentes = new List<string>();
            try
            {
                //Conexion a servicio
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                //Llamar al metodo
                var resultado = await cliente.ListOfCountryNamesGroupedByContinentAsync();

                await cliente.CloseAsync();

                //Retornar el resultado
                foreach (var item in resultado.Body.ListOfCountryNamesGroupedByContinentResult)
                {
                    //Agregar a la lista los parametros devueltos por el servicio
                    continentes.Add($"{item.Continent} - {item.CountryCodeAndNames}");
                }
            }
            catch (Exception ex)
            {
                continentes.Add($"Error al obtener continentes: {ex.Message}");
            }

            return continentes;
        }


        //Busca en la base de datos un país por el código de país ISO pasado
        public async Task<string> CountryName(string sCurrencyISOCode)
        {
            try
            {
                //Conexion a servicio
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                var resultado = await cliente.CountryNameAsync("AED");

                await cliente.CloseAsync();

                //Retornar el resultado
                return resultado.Body.CountryNameResult;
            }
            catch (Exception ex)
            {
                return $"Error al consultar la capital: {ex.Message}";
            }

        }


        //Esta función intenta encontrar un país basado en el nombre del país pasado.
        public async Task<string> CountryISOCode(string sCurrencyISOCode)
        {
            try
            {
                //Conexion a servicio
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                var resultado = await cliente.CountryISOCodeAsync("Afghanistan");

                await cliente.CloseAsync();

                //Retornar el resultado
                return resultado.Body.CountryISOCodeResult;
            }
            catch (Exception ex)
            {
                return $"Error al consultar la capital: {ex.Message}";
            }

        }


        //Devuelve el nombre de la ciudad capital para el código de país pasado
        public async Task<string> CapitalCity(string sCountryISOCode)
        {
            try
            {
                //Conexion a servicio
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                var resultado = await cliente.CapitalCityAsync("Afghanistan");

                await cliente.CloseAsync();

                //Retornar el resultado
                return resultado.Body.CapitalCityResult;
            }
            catch (Exception ex)
            {
                return $"Error al consultar la capital: {ex.Message}";
            }

        }


        //Devuelve el código ISO de la moneda y el nombre del código ISO del país pasado
        public async Task<string> CountryCurrency(string sCountryISOCode)
        {
            try
            {
                //Conexion a servicio
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                var resultado = await cliente.CountryCurrencyAsync("GT");

                await cliente.CloseAsync();

                //Retornar el resultado
                var moneda = resultado.Body.CountryCurrencyResult;

                return $"{moneda.sISOCode} ({moneda.sName})";

            }
            catch (Exception ex)
            {
                return $"Error al consultar la capital: {ex.Message}";
            }

        }


        //Devuelve un enlace a una imagen de la bandera del país
        public async Task<string> CountryFlag(string sCountryISOCode)
        {
            try
            {
                //Conexion a servicio
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                var resultado = await cliente.CountryFlagAsync("AED");

                await cliente.CloseAsync();

                //Retornar el resultado
                return resultado.Body.CountryFlagResult;
            }
            catch (Exception ex)
            {
                return $"Error al consultar la capital: {ex.Message}";
            }

        }


        //Devuelve el código telefónico internacional para el código de país ISO pasado
        public async Task<string> CountryIntPhoneCode(string sCountryISOCode)
        {
            try
            {
                //Conexion a servicio
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                var resultado = await cliente.CountryIntPhoneCodeAsync("GT");

                await cliente.CloseAsync();

                //Retornar el resultado
                return resultado.Body.CountryIntPhoneCodeResult;
            }
            catch (Exception ex)
            {
                return $"Error al consultar la capital: {ex.Message}";
            }

        }


        //Devuelve una estructura con toda la información de país almacenada. Pasar el código de país ISO
        public async Task<string> FullCountryInfo(string sCountryISOCode)
        {
            try
            {
                //Conexion a servicio
                var cliente = new CountryInfoServiceSoapTypeClient(
                    CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap
                );

                var resultado = await cliente.FullCountryInfoAsync("GT");

                await cliente.CloseAsync();

                //Retornar el resultado
                var info = resultado.Body.FullCountryInfoResult;

                return $"{info.sISOCode} ({info.sName}) ({info.sCapitalCity}) ({info.sPhoneCode}) ({info.sContinentCode}) ({info.sCurrencyISOCode}) ({info.sCountryFlag})";
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

                codigoPais = codigoPais.ToUpper();

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

using SoapDemoService;

namespace Examen_mes_abril.Services
{
    public interface ISoapDemoService
    {
        Task<string> ConsultarPersonaPorId(string id);
        Task<List<string>> ConsultarPersonasPorNombre(string nombre);
    }

    public class ServicioSOAPDemo : ISoapDemoService
    {
        //Consulta al personal por medio del id
        public async Task<string> ConsultarPersonaPorId(string id)
        {
            try
            {
                var cliente = new SOAPDemoSoapClient(SOAPDemoSoapClient.EndpointConfiguration.SOAPDemoSoap);

                var persona = await cliente.FindPersonAsync(id);

                await cliente.CloseAsync();

                return $"Nombre: {persona.Name}, Edad: {persona.Age}";
            }
            catch (Exception ex)
            {
                return $"Error al consultar persona: {ex.Message}";
            }
        }

        //Consulta a todo el personal que tenga el mismo nombre
        public async Task<List<string>> ConsultarPersonasPorNombre(string nombre)
        {
            var resultados = new List<string>();

            try
            {
                var cliente = new SOAPDemoSoapClient(SOAPDemoSoapClient.EndpointConfiguration.SOAPDemoSoap);

                var personas = await cliente.GetListByNameAsync(nombre);

                if (personas != null && personas.Length > 0)
                {
                    foreach (var persona in personas)
                    {
                        resultados.Add($"ID: {persona.ID}, Nombre: {persona.Name}, SSN: {persona.SSN}, Fecha de Nacimiento: {persona.DOB:yyyy-MM-dd}");
                    }
                }
                else
                {
                    resultados.Add("No se encontraron personas con ese nombre.");
                }

                await cliente.CloseAsync();
            }
            catch (Exception ex)
            {
                resultados.Add($"Error al consultar personas: {ex.Message}");
            }

            return resultados;
        }
    }
}

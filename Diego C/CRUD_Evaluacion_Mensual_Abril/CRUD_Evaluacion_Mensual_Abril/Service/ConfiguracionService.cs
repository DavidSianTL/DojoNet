using CRUD_Evaluacion_Mensual_Abril.Models;
using System.Text.Json;

namespace CRUD_Evaluacion_Mensual_Abril.Service
{
    public class ConfiguracionService
    {
        private readonly string rutaConfig = "wwwroot/data/configuracion.json";

        public ConfiguracionModel ObtenerConfiguracion()
        {
            if (!File.Exists(rutaConfig)) return new ConfiguracionModel { ColorPrincipal = "#9370DB" };

            var json = File.ReadAllText(rutaConfig);
            return JsonSerializer.Deserialize<ConfiguracionModel>(json);
        }
    }
}

using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using wAppGestionVacacional.Models;


namespace wAppGestionVacacional.Services
{
    public class EmpleadoService
    {

        private readonly string _empleadosFile = Path.Combine(Directory.GetCurrentDirectory(), "empleados.json");

        public List<Empleado> ObtenerEmpleados()
        {
            if (!File.Exists(_empleadosFile))
            {
                return new List<Empleado>();
            }

            var jsonData = File.ReadAllText(_empleadosFile);
            return JsonConvert.DeserializeObject<List<Empleado>>(jsonData) ?? new List<Empleado>();
        }

        public void GuardarEmpleado(Empleado empleado)
        {
            var empleados = ObtenerEmpleados();
            empleados.Add(empleado);
            File.WriteAllText(_empleadosFile, JsonConvert.SerializeObject(empleados, Formatting.Indented));
        }
    }
}

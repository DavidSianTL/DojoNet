using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using AutoExpress.Entidades.Models;
using AutoExpress.Negocios.Controllers;
using System.Diagnostics;

namespace AutoExpressSOAP.Services
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [ScriptService]
    public class VehiculoService : System.Web.Services.WebService
    {
        private readonly string _connectionString;
        private readonly VehiculoController _vehiculoController;

        public VehiculoService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _vehiculoController = new VehiculoController(_connectionString);
        }

        [WebMethod]
        public List<VehiculosViewModel> ObtenerTodos()
        {
            try
            {
                var resultado = _vehiculoController.ObtenerTodos();
                Console.WriteLine($"[Servicio] ObtenerTodos completado. Se encontraron {resultado?.Count ?? 0} vehículos");
                return resultado;
            }
            catch (Exception ex)
            {
                var errorMsg = $"[Servicio] Error en ObtenerTodos: {ex.Message}";
                throw new Exception(errorMsg, ex);
            }
        }

        [WebMethod]
        public VehiculosViewModel ObtenerPorId(int id)
        {
            Console.WriteLine($"[Servicio] Obteniendo vehículo con ID: {id}");
            var resultado = _vehiculoController.ObtenerPorId(id);
            Console.WriteLine($"[Servicio] Vehículo con ID {id} obtenido correctamente");
            return resultado;
        }

        [WebMethod]
        public string Crear(VehiculosViewModel vehiculo)
        {
            string mensaje;
            int id = _vehiculoController.Crear(vehiculo, out mensaje);

            if (id > 0)
            {
                Console.WriteLine($"[Servicio] {mensaje}");
            }
            else
            {
                Console.WriteLine($"[Servicio] Error al crear vehículo: {mensaje}");
            }

            return mensaje;
        }


        [WebMethod]
        public string Actualizar(VehiculosViewModel vehiculo)
        {
            string mensaje;
            bool resultado = _vehiculoController.Actualizar(vehiculo, out mensaje);

            if (resultado)
            {
                Console.WriteLine($"[Servicio] {mensaje}");
            }
            else
            {
                Console.WriteLine($"[Servicio] Error al actualizar vehículo: {mensaje}");
            }

            return mensaje;
        }

        [WebMethod]
        public string Eliminar(int id)
        {
            string mensaje;
            _vehiculoController.Eliminar(id, out mensaje);
            return mensaje;
        }

    }
}

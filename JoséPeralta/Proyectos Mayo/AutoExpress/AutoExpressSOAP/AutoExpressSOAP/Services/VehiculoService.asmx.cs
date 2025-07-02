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
                Debug.WriteLine($"[Servicio] ObtenerTodos completado. Se encontraron {resultado?.Count ?? 0} vehículos");
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
            try
            {
                Debug.WriteLine($"[Servicio] Obteniendo vehículo con ID: {id}");
                var resultado = _vehiculoController.ObtenerPorId(id);
                Debug.WriteLine($"[Servicio] Vehículo con ID {id} obtenido correctamente");
                return resultado;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Servicio] Error al obtener vehículo con ID {id}: {ex}");
                throw new Exception($"Error al obtener el vehículo con ID {id}: {ex.Message}", ex);
            }
        }

        [WebMethod]
        public int Crear(VehiculosViewModel vehiculo)
        {
            try
            {
                var id = _vehiculoController.Crear(vehiculo);
                Debug.WriteLine($"[Servicio] Vehículo creado con ID: {id}");
                return id;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Servicio] Error al crear vehículo: {ex}");
                throw new Exception($"Error al crear el vehículo: {ex.Message}", ex);
            }
        }

        [WebMethod]
        public bool Actualizar(VehiculosViewModel vehiculo)
        {
            try
            {
                Debug.WriteLine($"[Servicio] Actualizando vehículo con ID: {vehiculo?.IdVehiculo}");
                var resultado = _vehiculoController.Actualizar(vehiculo);
                Debug.WriteLine($"[Servicio] Vehículo con ID {vehiculo?.IdVehiculo} actualizado: {resultado}");
                return resultado;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Servicio] Error al actualizar vehículo con ID {vehiculo?.IdVehiculo}: {ex}");
                throw new Exception($"Error al actualizar el vehículo: {ex.Message}", ex);
            }
        }

        [WebMethod]
        public bool Eliminar(int id)
        {
            try
            {
                Debug.WriteLine($"[Servicio] Eliminando vehículo con ID: {id}");
                var resultado = _vehiculoController.Eliminar(id);
                Debug.WriteLine($"[Servicio] Vehículo con ID {id} eliminado: {resultado}");
                return resultado;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Servicio] Error al eliminar vehículo con ID {id}: {ex}");
                throw new Exception($"Error al eliminar el vehículo con ID {id}: {ex.Message}", ex);
            }
        }
    }
}

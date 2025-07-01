using AutoExpress.Entidades.Modelos;
using AutoExpress.Negocio.Reglas;
using System.Collections.Generic;
using System.Web.Services;

namespace AutoExpress.Servicios
{
    /// <summary>
    /// Servicio web para gestión de vehículos en AutoExpress.
    /// </summary>
    [WebService(Namespace = "http://autoexpress.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class VehiculoService : WebService
    {
        private VehiculoLogica logica = new VehiculoLogica();

        [WebMethod(Description = "Obtiene la lista de todos los vehículos.")]
        public List<Vehiculo> ObtenerVehiculos()
        {
            return logica.ObtenerTodos();
        }

        [WebMethod(Description = "Agrega un nuevo vehículo a la base de datos.")]
        public RespuestaOperacion AgregarVehiculo(Vehiculo v)
        {
            return logica.Agregar(v);
        }

        [WebMethod(Description = "Edita la información de un vehículo existente.")]
        public RespuestaOperacion EditarVehiculo(Vehiculo v)
        {
            return logica.Editar(v);
        }

        [WebMethod(Description = "Cambia el estado (activo/inactivo) de un vehículo.")]
        public RespuestaOperacion CambiarEstadoVehiculo(int idVehiculo, int nuevoEstado)
        {
            return logica.CambiarEstado(idVehiculo, nuevoEstado);
        }
    }
}

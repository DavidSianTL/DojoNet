using System;
using System.Collections.Generic;
using System.Web.Services;
using AutoExpress.Entidades;
using AutoExpress.Negocio;

namespace AutoExpress.Servicio
{
    /// <summary>
    /// Servicio web SOAP para la gestión de carros de AutoExpress Guatemala
    /// </summary>
    [WebService(Namespace = "http://autoexpress.guatemala.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class CarroService : System.Web.Services.WebService
    {
        private CarroNegocio carroNegocio;

        public CarroService()
        {
            carroNegocio = new CarroNegocio();
        }

        /// <summary>
        /// Lista todos los vehículos registrados en el sistema
        /// </summary>
        /// <returns>Lista de carros</returns>
        [WebMethod(Description = "Obtiene la lista completa de vehículos registrados")]
        public RespuestaServicio ListarCarros()
        {
            try
            {
                return carroNegocio.ListarCarros();
            }
            catch (Exception ex)
            {
                return new RespuestaServicio(false, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene un vehículo específico por su ID
        /// </summary>
        /// <param name="id">ID del vehículo</param>
        /// <returns>Información del vehículo</returns>
        [WebMethod(Description = "Obtiene un vehículo específico por su ID")]
        public RespuestaServicio ObtenerCarro(int id)
        {
            try
            {
                return carroNegocio.ObtenerCarro(id);
            }
            catch (Exception ex)
            {
                return new RespuestaServicio(false, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Agrega un nuevo vehículo al sistema
        /// </summary>
        /// <param name="marca">Marca del vehículo</param>
        /// <param name="modelo">Modelo del vehículo</param>
        /// <param name="año">Año del vehículo</param>
        /// <param name="precio">Precio del vehículo</param>
        /// <param name="disponible">Indica si el vehículo está disponible</param>
        /// <returns>Resultado de la operación</returns>
        [WebMethod(Description = "Agrega un nuevo vehículo al sistema")]
        public RespuestaServicio AgregarCarro(string marca, string modelo, int año, decimal precio, bool disponible = true)
        {
            try
            {
                Carro nuevoCarro = new Carro(marca, modelo, año, precio, disponible);
                return carroNegocio.AgregarCarro(nuevoCarro);
            }
            catch (Exception ex)
            {
                return new RespuestaServicio(false, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Edita la información de un vehículo existente
        /// </summary>
        /// <param name="id">ID del vehículo a editar</param>
        /// <param name="marca">Nueva marca del vehículo</param>
        /// <param name="modelo">Nuevo modelo del vehículo</param>
        /// <param name="año">Nuevo año del vehículo</param>
        /// <param name="precio">Nuevo precio del vehículo</param>
        /// <param name="disponible">Nueva disponibilidad del vehículo</param>
        /// <returns>Resultado de la operación</returns>
        [WebMethod(Description = "Edita la información de un vehículo existente")]
        public RespuestaServicio EditarCarro(int id, string marca, string modelo, int año, decimal precio, bool disponible)
        {
            try
            {
                Carro carro = new Carro(marca, modelo, año, precio, disponible)
                {
                    Id = id
                };
                return carroNegocio.EditarCarro(carro);
            }
            catch (Exception ex)
            {
                return new RespuestaServicio(false, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Elimina un vehículo del sistema
        /// </summary>
        /// <param name="id">ID del vehículo a eliminar</param>
        /// <returns>Resultado de la operación</returns>
        [WebMethod(Description = "Elimina un vehículo del sistema")]
        public RespuestaServicio EliminarCarro(int id)
        {
            try
            {
                return carroNegocio.EliminarCarro(id);
            }
            catch (Exception ex)
            {
                return new RespuestaServicio(false, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Método de prueba para verificar que el servicio está funcionando
        /// </summary>
        /// <returns>Mensaje de confirmación</returns>
        [WebMethod(Description = "Método de prueba para verificar conectividad")]
        public string Ping()
        {
            return "Servicio AutoExpress funcionando correctamente - " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}

    using AutoExpress.Datos.DAO;
using AutoExpress.Entidades.Modelos;
using System.Collections.Generic;

namespace AutoExpress.Negocio.Reglas
{
    public class VehiculoLogica
    {
        private VehiculoDAO dao = new VehiculoDAO();

        public List<Vehiculo> ObtenerTodos()
        {
            return dao.Listar();
        }

        public RespuestaOperacion Agregar(Vehiculo v)
        {
            if (v.Anio < 2000)
            {
                return new RespuestaOperacion
                {
                    Exito = false,
                    Mensaje = "El año debe ser mayor o igual a 2000."
                };
            }

            if (v.Precio <= 0)
            {
                return new RespuestaOperacion
                {
                    Exito = false,
                    Mensaje = "El precio debe ser mayor a cero."
                };
            }

            bool resultado = dao.Agregar(v);

            return new RespuestaOperacion
            {
                Exito = resultado,
                Mensaje = resultado ? "Vehículo agregado correctamente." : "No se pudo agregar el vehículo."
            };
        }

        public RespuestaOperacion Editar(Vehiculo v)
        {
            if (v.Anio < 2000)
            {
                return new RespuestaOperacion
                {
                    Exito = false,
                    Mensaje = "El año debe ser mayor o igual a 2000."
                };
            }

            if (v.Precio <= 0)
            {
                return new RespuestaOperacion
                {
                    Exito = false,
                    Mensaje = "El precio debe ser mayor a cero."
                };
            }

            bool resultado = dao.Editar(v);

            return new RespuestaOperacion
            {
                Exito = resultado,
                Mensaje = resultado ? "Vehículo editado correctamente." : "No se pudo editar el vehículo."
            };
        }

        public RespuestaOperacion CambiarEstado(int idVehiculo, int nuevoEstado)
        {
            if (idVehiculo <= 0 || nuevoEstado <= 0)
            {
                return new RespuestaOperacion
                {
                    Exito = false,
                    Mensaje = "ID de vehículo o estado inválido."
                };
            }

            bool resultado = dao.CambiarEstado(idVehiculo, nuevoEstado);

            return new RespuestaOperacion
            {
                Exito = resultado,
                Mensaje = resultado ? "Estado actualizado correctamente." : "No se pudo actualizar el estado."
            };
        }

    }
}

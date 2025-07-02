using System;
using System.Collections.Generic;
using AutoExpress.Datos.Dao;
using AutoExpress.Entidades.Models;
using System.Configuration; // Permite leer configuraciones del archivo web.config
using System.Diagnostics;

namespace AutoExpress.Negocios.Controllers
{
    public class VehiculoController
    {
        private readonly daoVehiculo _vehiculoDao;
        private readonly string _connectionString;

        // Constructor por defecto que lee la cadena de conexión desde web.config
        public VehiculoController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _vehiculoDao = new daoVehiculo(_connectionString);
        }

        // Constructor que permite pasar la cadena de conexión como parámetro
        public VehiculoController(string connectionString)
        {
            _connectionString = connectionString;
            _vehiculoDao = new daoVehiculo(connectionString);
        }

        // Obtiene todos los vehículos de la base de datos
        public List<VehiculosViewModel> ObtenerTodos()
        {
            try
            {
                // Se obtienen los datos desde el DAO
                var resultado = _vehiculoDao.ObtenerTodos();

                return resultado;
            }
            catch (Exception ex)
            {
                var errorMsg = $"[Controlador] Error al obtener vehículos: {ex.Message}";

                // Lanza excepción personalizada con detalle
                throw new Exception(errorMsg, ex);
            }
        }


        // Obtiene un vehículo por su ID
        public VehiculosViewModel ObtenerPorId(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("El ID del vehículo no es válido");
                }

                return _vehiculoDao.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                // Lanza excepción personalizada con detalle
                throw new Exception($"Error al obtener el vehículo con ID {id}: {ex.Message}", ex);
            }
        }


        // Crea un nuevo vehículo en la base de datos
        public int Crear(VehiculosViewModel vehiculo, out string mensaje)
        {
            try
            {
                // Validación de objeto nulo
                if (vehiculo == null)
                {
                    throw new ArgumentNullException(nameof(vehiculo), "El vehículo no puede ser nulo");
                }

                // Validación de año
                if (vehiculo.Anio < 1900 || vehiculo.Anio > DateTime.Now.Year + 1)
                {
                    throw new ArgumentException("El año del vehículo no es válido");
                }

                // Validación de precio
                if (vehiculo.Precio <= 0)
                {
                    throw new ArgumentException("El precio del vehículo debe ser mayor a cero");
                }

                // Se llama al método del DAO para insertar
                return _vehiculoDao.Insertar(vehiculo, out mensaje);
            }
            catch (Exception ex)
            {
                mensaje = $"Error en el controlador al crear vehículo: {ex.Message}";
                return -1;
            }
        }


        // Actualiza los datos de un vehículo existente
        public bool Actualizar(VehiculosViewModel vehiculo, out string mensaje)
        {
            try
            {
                if (vehiculo == null)
                {
                    throw new ArgumentNullException(nameof(vehiculo), "El vehículo no puede ser nulo");
                }

                if (vehiculo.IdVehiculo <= 0)
                {
                    throw new ArgumentException("El ID del vehículo no es válido");
                }

                if (vehiculo.Anio < 1900 || vehiculo.Anio > DateTime.Now.Year + 1)
                {
                    throw new ArgumentException("El año del vehículo no es válido");
                }

                if (vehiculo.Precio <= 0)
                {
                    throw new ArgumentException("El precio del vehículo debe ser mayor a cero");
                }

                // Llama al método del DAO para actualizar
                return _vehiculoDao.Actualizar(vehiculo, out mensaje);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el vehículo con ID {vehiculo?.IdVehiculo}: {ex.Message}", ex);
            }
        }


        // Elimina (o cambia de estado) a un vehículo según su ID
        public bool Eliminar(int id, out string mensaje)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("El ID del vehículo no es válido");
                }

                // Llama al DAO para marcar como retirado
                return _vehiculoDao.Eliminar(id, out mensaje);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar el vehículo con ID {id}: {ex.Message}", ex);
            }
        }

    }
}

using System;
using System.Collections.Generic;
using AutoExpress.Datos.Dao;
using AutoExpress.Entidades.Models;
using System.Configuration;
using System.Diagnostics;

namespace AutoExpress.Negocios.Controllers
{
    public class VehiculoController
    {
        private readonly daoVehiculo _vehiculoDao;
        private readonly string _connectionString;

        public VehiculoController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            _vehiculoDao = new daoVehiculo(_connectionString);
        }

        public VehiculoController(string connectionString)
        {
            _connectionString = connectionString;
            _vehiculoDao = new daoVehiculo(connectionString);
        }

        public List<VehiculosViewModel> ObtenerTodos()
        {
            try
            {
                Debug.WriteLine("[Controlador] Iniciando ObtenerTodos");
                var resultado = _vehiculoDao.ObtenerTodos();
                Debug.WriteLine($"[Controlador] Se obtuvieron {resultado?.Count ?? 0} vehículos");
                return resultado;
            }
            catch (Exception ex)
            {
                var errorMsg = $"[Controlador] Error al obtener vehículos: {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMsg += $" - Inner: {ex.InnerException.Message}";
                }
                Debug.WriteLine(errorMsg);
                throw new Exception(errorMsg, ex);
            }
        }

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
                Debug.WriteLine($"[Controlador] Error al obtener vehículo con ID {id}: {ex}");
                throw new Exception($"Error al obtener el vehículo con ID {id}: {ex.Message}", ex);
            }
        }

        public int Crear(VehiculosViewModel vehiculo)
        {
            try
            {
                if (vehiculo == null)
                {
                    throw new ArgumentNullException(nameof(vehiculo), "El vehículo no puede ser nulo");
                }

                if (vehiculo.Anio < 1900 || vehiculo.Anio > DateTime.Now.Year + 1)
                {
                    throw new ArgumentException("El año del vehículo no es válido");
                }

                if (vehiculo.Precio <= 0)
                {
                    throw new ArgumentException("El precio del vehículo debe ser mayor a cero");
                }

                return _vehiculoDao.Insertar(vehiculo);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Controlador] Error al crear vehículo: {ex}");
                throw new Exception("Error al crear el vehículo: " + ex.Message, ex);
            }
        }

        public bool Actualizar(VehiculosViewModel vehiculo)
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

                return _vehiculoDao.Actualizar(vehiculo);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Controlador] Error al actualizar vehículo con ID {vehiculo?.IdVehiculo}: {ex}");
                throw new Exception($"Error al actualizar el vehículo con ID {vehiculo?.IdVehiculo}: {ex.Message}", ex);
            }
        }

        public bool Eliminar(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentException("El ID del vehículo no es válido");
                }

                return _vehiculoDao.Eliminar(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Controlador] Error al eliminar vehículo con ID {id}: {ex}");
                throw new Exception($"Error al eliminar el vehículo con ID {id}: {ex.Message}", ex);
            }
        }
    }
}

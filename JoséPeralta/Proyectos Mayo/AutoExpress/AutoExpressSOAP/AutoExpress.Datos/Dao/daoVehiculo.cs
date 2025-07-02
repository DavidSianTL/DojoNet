using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using AutoExpress.Entidades.Models;

namespace AutoExpress.Datos.Dao
{
    public class daoVehiculo
    {
        private readonly string _connectionString;

        public daoVehiculo(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Obtener todos los vehículos
        public List<VehiculosViewModel> ObtenerTodos()
        {
            var vehiculos = new List<VehiculosViewModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("sp_ObtenerVehiculos", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            vehiculos.Add(new VehiculosViewModel
                            {
                                IdVehiculo = reader.GetInt32(reader.GetOrdinal("idVehiculo")),
                                Marca = reader.GetString(reader.GetOrdinal("marca")),
                                Modelo = reader.GetString(reader.GetOrdinal("modelo")),
                                Anio = reader.GetInt32(reader.GetOrdinal("anio")),
                                Precio = reader.GetDecimal(reader.GetOrdinal("precio")),
                                IdTipoVehiculo = reader.GetInt32(reader.GetOrdinal("fk_IdTipoVehiculo")),
                                IdEstado = reader.GetInt32(reader.GetOrdinal("fk_IdEstado")),
                                TipoVehiculo = new TipoVehiculosViewModel
                                {
                                    IdTipoVehiculo = reader.GetInt32(reader.GetOrdinal("fk_IdTipoVehiculo")),
                                    Tipo = reader.GetString(reader.GetOrdinal("tipo"))
                                },
                                Estado = new EstadosViewModel
                                {
                                    IdEstado = reader.GetInt32(reader.GetOrdinal("fk_IdEstado")),
                                    EstadoNombre = reader.GetString(reader.GetOrdinal("estado"))
                                }
                            });
                        }
                    }
                }
            }
            return vehiculos;
        }

        // Obtener un vehículo por ID
        public VehiculosViewModel ObtenerPorId(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_ObtenerVehiculoPorId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdVehiculo", id);

                    using (var reader = command.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (reader.Read())
                        {
                            return new VehiculosViewModel
                            {
                                IdVehiculo = reader.GetInt32(reader.GetOrdinal("idVehiculo")),
                                Marca = reader.GetString(reader.GetOrdinal("marca")),
                                Modelo = reader.GetString(reader.GetOrdinal("modelo")),
                                Anio = reader.GetInt32(reader.GetOrdinal("anio")),
                                Precio = reader.GetDecimal(reader.GetOrdinal("precio")),
                                IdTipoVehiculo = reader.GetInt32(reader.GetOrdinal("IdTipoVehiculo")),
                                IdEstado = reader.GetInt32(reader.GetOrdinal("IdEstado")),
                                TipoVehiculo = new TipoVehiculosViewModel
                                {
                                    IdTipoVehiculo = reader.GetInt32(reader.GetOrdinal("IdTipoVehiculo")),
                                    Tipo = reader.GetString(reader.GetOrdinal("TipoVehiculo"))
                                },
                                Estado = new EstadosViewModel
                                {
                                    IdEstado = reader.GetInt32(reader.GetOrdinal("IdEstado")),
                                    EstadoNombre = reader.GetString(reader.GetOrdinal("EstadoNombre"))
                                }
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Insertar un nuevo vehículo
        public int Insertar(VehiculosViewModel vehiculo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_InsertarVehiculo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Marca", vehiculo.Marca);
                    command.Parameters.AddWithValue("@Modelo", vehiculo.Modelo);
                    command.Parameters.AddWithValue("@Anio", vehiculo.Anio);
                    command.Parameters.AddWithValue("@Precio", vehiculo.Precio);
                    command.Parameters.AddWithValue("@IdTipoVehiculo", vehiculo.IdTipoVehiculo);
                    command.Parameters.AddWithValue("@IdEstado", vehiculo.IdEstado);

                    var idParameter = new SqlParameter("@IdVehiculo", SqlDbType.Int) 
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(idParameter);

                    command.ExecuteNonQuery();
                    return (int)idParameter.Value;
                }
            }
        }

        // Actualizar un vehículo existente
        public bool Actualizar(VehiculosViewModel vehiculo) 
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_ActualizarVehiculo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdVehiculo", vehiculo.IdVehiculo);
                    command.Parameters.AddWithValue("@Marca", vehiculo.Marca);
                    command.Parameters.AddWithValue("@Modelo", vehiculo.Modelo);
                    command.Parameters.AddWithValue("@Anio", vehiculo.Anio);
                    command.Parameters.AddWithValue("@Precio", vehiculo.Precio);
                    command.Parameters.AddWithValue("@IdTipoVehiculo", vehiculo.IdTipoVehiculo);
                    command.Parameters.AddWithValue("@IdEstado", vehiculo.IdEstado);

                    var rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // Eliminar un vehículo
        public bool Eliminar(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_EliminarVehiculo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdVehiculo", id);

                    var rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}

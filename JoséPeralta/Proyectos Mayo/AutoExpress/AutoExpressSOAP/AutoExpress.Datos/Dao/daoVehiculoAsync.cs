using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AutoExpress.Entidades.Models;

namespace AutoExpress.Datos.Dao
{
    public class daoVehiculoAsync
    {
        private readonly string _connectionString;

        public daoVehiculoAsync(string connectionString)
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

        // Versión asíncrona (mantenida por compatibilidad)
        public async Task<List<VehiculosViewModel>> ObtenerTodosAsync()
        {
            return await Task.Run(() => ObtenerTodos());
        }

        // Obtener un vehículo por ID
        public async Task<VehiculosViewModel> ObtenerPorIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_ObtenerVehiculoPorId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdVehiculo", id);

                    using (var reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow))
                    {
                        if (await reader.ReadAsync())
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
        public async Task<int> InsertarAsync(VehiculosViewModel vehiculo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
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

                    await command.ExecuteNonQueryAsync();
                    return (int)idParameter.Value;
                }
            }
        }

        // Actualizar un vehículo existente
        public async Task<bool> ActualizarAsync(VehiculosViewModel vehiculo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
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

                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        // Eliminar un vehículo
        public async Task<bool> EliminarAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("sp_EliminarVehiculo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdVehiculo", id);

                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
    }
}

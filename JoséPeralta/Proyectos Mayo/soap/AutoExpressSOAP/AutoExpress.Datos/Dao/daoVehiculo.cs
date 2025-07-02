using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using AutoExpress.Entidades.Models;

namespace AutoExpress.Datos.Dao
{
    public class daoVehiculo
    {
        // Cadena de conexión a la base de datos
        private readonly string _connectionString;

        // Constructor que recibe la cadena de conexión
        public daoVehiculo(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener todos los vehículos registrados en la base de datos
        public List<VehiculosViewModel> ObtenerTodos()
        {
            var vehiculos = new List<VehiculosViewModel>(); // Lista donde se almacenarán los vehículos

            using (var connection = new SqlConnection(_connectionString)) // Crear conexión
            {
                connection.Open(); // Abrir conexión

                using (var command = new SqlCommand("sp_ObtenerVehiculos", connection)) // Usar SP
                {
                    command.CommandType = CommandType.StoredProcedure; // Tipo SP

                    using (var reader = command.ExecuteReader()) // Ejecutar reader
                    {
                        while (reader.Read()) // Leer filas una a una
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
            return vehiculos; // Retornar lista de vehículos
        }

        // Método para obtener un vehículo específico por su ID
        public VehiculosViewModel ObtenerPorId(int id)
        {
            using (var connection = new SqlConnection(_connectionString)) // Crear conexión
            {
                connection.Open(); // Abrir conexión
                using (var command = new SqlCommand("sp_ObtenerVehiculoPorId", connection)) // SP específico
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdVehiculo", id); // Parámetro ID

                    using (var reader = command.ExecuteReader(CommandBehavior.SingleRow)) // Leer solo una fila
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
            return null; // Si no se encuentra, retorna null
        }

        // Método para insertar un nuevo vehículo en la base de datos
        public int Insertar(VehiculosViewModel vehiculo, out string mensaje)
        {
            mensaje = string.Empty; // Inicializar mensaje

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_InsertarVehiculo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    // Agregar parámetros
                    command.Parameters.AddWithValue("@Marca", vehiculo.Marca);
                    command.Parameters.AddWithValue("@Modelo", vehiculo.Modelo);
                    command.Parameters.AddWithValue("@Anio", vehiculo.Anio);
                    command.Parameters.AddWithValue("@Precio", vehiculo.Precio);
                    command.Parameters.AddWithValue("@IdTipoVehiculo", vehiculo.IdTipoVehiculo);
                    command.Parameters.AddWithValue("@IdEstado", vehiculo.IdEstado);

                    try
                    {
                        var id = Convert.ToInt32(command.ExecuteScalar()); // Obtener ID generado
                        mensaje = $"Vehículo {vehiculo.Marca} {vehiculo.Modelo} creado exitosamente con ID: {id}";
                        return id;
                    }
                    catch (Exception ex)
                    {
                        mensaje = $"Error al crear el vehículo: {ex.Message}";
                        return -1;
                    }
                }
            }
        }

        // Método para actualizar un vehículo existente
        public bool Actualizar(VehiculosViewModel vehiculo, out string mensaje)
        {
            mensaje = string.Empty;

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Verificar si el vehículo existe
                using (var cmdVerificar = new SqlCommand("SELECT 1 FROM Vehiculos WHERE idVehiculo = @Id", connection))
                {
                    cmdVerificar.Parameters.AddWithValue("@Id", vehiculo.IdVehiculo);
                    if (cmdVerificar.ExecuteScalar() == null)
                    {
                        mensaje = $"No se encontró ningún vehículo con el ID {vehiculo.IdVehiculo}.";
                        return false;
                    }
                }

                // Si existe, actualizar
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

                    try
                    {
                        int filasAfectadas = command.ExecuteNonQuery();
                        if (filasAfectadas > 0)
                        {
                            mensaje = $"Vehículo actualizado correctamente: {vehiculo.Marca} {vehiculo.Modelo} (ID: {vehiculo.IdVehiculo})";
                            return true;
                        }
                        else
                        {
                            mensaje = "No se realizaron cambios en el vehículo.";
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        mensaje = $"Error al actualizar el vehículo: {ex.Message}";
                        return false;
                    }
                }
            }
        }

        // Método para eliminar (retirar) un vehículo
        public bool Eliminar(int id, out string mensaje)
        {
            mensaje = string.Empty; // Inicializar mensaje

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("sp_EliminarVehiculo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@IdVehiculo", id);

                    // Parámetro de retorno
                    var returnParameter = command.Parameters.Add("@ReturnVal", SqlDbType.Int);
                    returnParameter.Direction = ParameterDirection.ReturnValue;

                    command.ExecuteNonQuery(); // Ejecutar

                    int filasAfectadas = (int)returnParameter.Value;

                    if (filasAfectadas > 0)
                    {
                        // Obtener datos del vehículo retirado para el mensaje
                        using (var cmdDetalle = new SqlCommand("SELECT marca, modelo FROM Vehiculos WHERE idVehiculo = @Id", connection))
                        {
                            cmdDetalle.Parameters.AddWithValue("@Id", id);
                            using (var reader = cmdDetalle.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string marca = reader["marca"].ToString();
                                    string modelo = reader["modelo"].ToString();
                                    mensaje = $"Vehículo retirado exitosamente: {marca} {modelo} (ID: {id})";
                                }
                            }
                        }
                        return true;
                    }
                    else
                    {
                        // Verificar si ya estaba retirado o no existe
                        using (var cmdVerificar = new SqlCommand("SELECT 1 FROM Vehiculos WHERE idVehiculo = @Id", connection))
                        {
                            cmdVerificar.Parameters.AddWithValue("@Id", id);
                            bool existe = cmdVerificar.ExecuteScalar() != null;

                            mensaje = existe ? "El vehículo ya se encuentra retirado." : $"No se encontró ningún vehículo con el ID {id}.";
                        }
                        return false;
                    }
                }
            }
        }
    }
}

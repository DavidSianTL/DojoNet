using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using RetoCRUDEmpleados.Models;

namespace RetoCRUDEmpleados.Services
{
    public class EmpleadoDAO
    {
        private readonly string _connectionString;

        public EmpleadoDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Obtener todos los empleados
        public async Task<List<EmpleadoViewModel>> ObtenerEmpleadosAsync()
        {
            var empleados = new List<EmpleadoViewModel>();
            string query = "SELECT * FROM Empleados";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        empleados.Add(new EmpleadoViewModel
                        {
                            IdEmpleado = reader.GetInt32(0),
                            Nombre = reader.GetString(1),
                            Apellido = reader.GetString(2),
                            FechaNacimiento = reader.GetDateTime(3),
                            FechaIngreso = reader.GetDateTime(4),
                            Puesto = reader.GetString(5),
                            SalarioBase = reader.GetDecimal(6),
                            Activo = reader.GetBoolean(7)
                        });
                    }
                }
            }

            return empleados;
        }

        // Insertar un nuevo empleado
        public async Task InsertarEmpleadoAsync(EmpleadoViewModel empleado)
        {
            string sql = @"INSERT INTO Empleados 
                           (Nombre, Apellido, FechaNacimiento, FechaIngreso, Puesto, SalarioBase, Activo) 
                           VALUES 
                           (@Nombre, @Apellido, @FechaNacimiento, @FechaIngreso, @Puesto, @SalarioBase, @Activo)";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", empleado.Apellido);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", empleado.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@FechaIngreso", empleado.FechaIngreso);
                    cmd.Parameters.AddWithValue("@Puesto", empleado.Puesto);
                    cmd.Parameters.AddWithValue("@SalarioBase", empleado.SalarioBase);
                    cmd.Parameters.AddWithValue("@Activo", empleado.Activo);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Obtener un empleado por ID
        public async Task<EmpleadoViewModel> ObtenerEmpleadoPorIdAsync(int id)
        {
            EmpleadoViewModel empleado = null;
            string sql = "SELECT * FROM Empleados WHERE IdEmpleado = @Id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            empleado = new EmpleadoViewModel
                            {
                                IdEmpleado = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Apellido = reader.GetString(2),
                                FechaNacimiento = reader.GetDateTime(3),
                                FechaIngreso = reader.GetDateTime(4),
                                Puesto = reader.GetString(5),
                                SalarioBase = reader.GetDecimal(6),
                                Activo = reader.GetBoolean(7)
                            };
                        }
                    }
                }
            }

            return empleado;
        }

        // Actualizar un empleado
        public async Task ActualizarEmpleadoAsync(EmpleadoViewModel empleado)
        {
            string sql = @"UPDATE Empleados SET 
                           Nombre = @Nombre, 
                           Apellido = @Apellido, 
                           FechaNacimiento = @FechaNacimiento,
                           FechaIngreso = @FechaIngreso,
                           Puesto = @Puesto,
                           SalarioBase = @SalarioBase,
                           Activo = @Activo
                           WHERE IdEmpleado = @Id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", empleado.Apellido);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", empleado.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@FechaIngreso", empleado.FechaIngreso);
                    cmd.Parameters.AddWithValue("@Puesto", empleado.Puesto);
                    cmd.Parameters.AddWithValue("@SalarioBase", empleado.SalarioBase);
                    cmd.Parameters.AddWithValue("@Activo", empleado.Activo);
                    cmd.Parameters.AddWithValue("@Id", empleado.IdEmpleado);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Eliminar un empleado
        public async Task EliminarEmpleadoAsync(int id)
        {
            string sql = "DELETE FROM Empleados WHERE IdEmpleado = @Id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

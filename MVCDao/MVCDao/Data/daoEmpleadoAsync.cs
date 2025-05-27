using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using MVCDao.Models; // Asegúrate de que esta ruta sea la correcta

namespace MVCDao.Data
{
    public class daoEmpleadoAsync
    {
        private readonly string _connectionString;

        public daoEmpleadoAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Empleado>> ObtenerEmpleadosAsync()
        {
            var empleados = new List<Empleado>();
            string sql = "SELECT EmpleadoID, Nombre, Apellido, FechaNacimiento, FechaIngreso, Puesto, SalarioBase, Activo FROM Empleados";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            empleados.Add(new Empleado
                            {
                                EmpleadoID = reader.GetInt32(0),
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
            }
            return empleados;
        }

        public async Task<Empleado> ObtenerEmpleadoPorIdAsync(int id)
        {
            Empleado empleado = null;
            string sql = "SELECT EmpleadoID, Nombre, Apellido, FechaNacimiento, FechaIngreso, Puesto, SalarioBase, Activo FROM Empleados WHERE EmpleadoID = @EmpleadoID";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EmpleadoID", id);
                    await conn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            empleado = new Empleado
                            {
                                EmpleadoID = reader.GetInt32(0),
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

        public async Task InsertarEmpleadoAsync(Empleado empleado)
        {
            string sql = @"
                INSERT INTO Empleados (Nombre, Apellido, FechaNacimiento, FechaIngreso, Puesto, SalarioBase, Activo)
                VALUES (@Nombre, @Apellido, @FechaNacimiento, @FechaIngreso, @Puesto, @SalarioBase, @Activo)";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", empleado.Apellido);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", empleado.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@FechaIngreso", empleado.FechaIngreso);
                    cmd.Parameters.AddWithValue("@Puesto", empleado.Puesto);
                    cmd.Parameters.AddWithValue("@SalarioBase", empleado.SalarioBase);
                    cmd.Parameters.AddWithValue("@Activo", empleado.Activo);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task ActualizarEmpleadoAsync(Empleado empleado)
        {
            string sql = @"
                UPDATE Empleados
                SET Nombre = @Nombre,
                    Apellido = @Apellido,
                    FechaNacimiento = @FechaNacimiento,
                    FechaIngreso = @FechaIngreso,
                    Puesto = @Puesto,
                    SalarioBase = @SalarioBase,
                    Activo = @Activo
                WHERE EmpleadoID = @EmpleadoID";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EmpleadoID", empleado.EmpleadoID);
                    cmd.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", empleado.Apellido);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", empleado.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@FechaIngreso", empleado.FechaIngreso);
                    cmd.Parameters.AddWithValue("@Puesto", empleado.Puesto);
                    cmd.Parameters.AddWithValue("@SalarioBase", empleado.SalarioBase);
                    cmd.Parameters.AddWithValue("@Activo", empleado.Activo);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task EliminarEmpleadoAsync(int id)
        {
            string sql = "DELETE FROM Empleados WHERE EmpleadoID = @EmpleadoID";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EmpleadoID", id);
                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

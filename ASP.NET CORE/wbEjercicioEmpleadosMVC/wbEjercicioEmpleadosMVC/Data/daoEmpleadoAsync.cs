using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using wbEjercicioEmpleadosMVC.Models;


namespace wbEjercicioEmpleadosMVC.Data
{
    public class daoEmpleadoAsync
    {
        private readonly string _connectionString;

        public daoEmpleadoAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<EmpleadoModel>> ObtenerEmpleadoAsync()
        {
            var Empleados= new List<EmpleadoModel>();

            string query = "SELECT empleadoId, nombre, apellido, fechaNacimiento, fechaIngreso, puesto, salarioBase, activo FROM Empleados";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Empleados.Add(new EmpleadoModel() 
                            {
                                EmpleadoID = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                Apellido = reader.GetString(2),
                                FechaNacimiento = reader.GetDateTime(3),
                                FechaIngreso = reader.GetDateTime(4),
                                Puesto= reader.GetString(5),
                                SalarioBase= reader.GetDecimal(6),
                                Activo= reader.GetBoolean(7)
                            });
                        }
                    }

                }

            }
            return Empleados;
        }
        public async Task InsertarEmpleadoAsync(EmpleadoModel empleado)
        {
            string sql = "INSERT INTO Empleados (nombre, apellido, fechaNacimiento, fechaIngreso, puesto, salarioBase, activo) VALUES (@nombre, @apellido, @fechaNacimiento, @fechaIngreso, @puesto, @salarioBase, @activo )";

            using (SqlConnection cnn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    cmd.Parameters.AddWithValue("@nombre", empleado.Nombre);
                    cmd.Parameters.AddWithValue("@apellido", empleado.Apellido);
                    cmd.Parameters.AddWithValue("@fechaNacimiento", empleado.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@fechaIngreso", empleado.FechaIngreso);
                    cmd.Parameters.AddWithValue("@puesto", empleado.Puesto);
                    cmd.Parameters.AddWithValue("@salarioBase", empleado.SalarioBase);
                    cmd.Parameters.AddWithValue("@activo", empleado.Activo);

                    await cnn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                }


            }
        }

        public async Task<EmpleadoModel> ObtenerEmpleadoPorIdAsync(int Id)
        {
            EmpleadoModel empleado = null;

            string sql = "SELECT empleadoId, nombre, apellido, fechaNacimiento, fechaIngreso, puesto, salarioBase, activo FROM Empleados WHERE empleadoId= @Id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);
                    await conn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            empleado = new EmpleadoModel
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

        public async Task ActualizarEmpleadoAsync(EmpleadoModel empleado)
        {
            string sql = "UPDATE Empleados SET nombre = @nombre, apellido = @apellido, fechaNacimiento = @fechaNacimiento, fechaIngreso = @fechaIngreso, puesto = @puesto, salarioBase = @salarioBase, activo= @activo WHERE empleadoId = @Id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", empleado.Nombre);
                    cmd.Parameters.AddWithValue("@apellido", empleado.Apellido);
                    cmd.Parameters.AddWithValue("@fechaNacimiento", empleado.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@fechaIngreso", empleado.FechaIngreso);
                    cmd.Parameters.AddWithValue("@puesto", empleado.Puesto);
                    cmd.Parameters.AddWithValue("@salarioBase", empleado.SalarioBase);
                    cmd.Parameters.AddWithValue("@activo", empleado.Activo);
                    cmd.Parameters.AddWithValue("@Id", empleado.EmpleadoID);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
            }

        }

        public async Task EliminarEmpleadoAsync(int Id)
        {
            string sql = "UPDATE Empleados SET Activo = 0 WHERE empleadoId = @Id;";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {

                    cmd.Parameters.AddWithValue("@Id", Id);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();


                }
            }
        }
    }
}

using CurdconcnnConexionAsync.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

public class EmpleadoDAO
{
    private string connectionString = "Server=DARLA\\SQLEXPRESS01;Database=EmpresaDB;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;";

    public async Task<List<Empleado>> ObtenerEmpleadosAsync()
    {
        var empleados = new List<Empleado>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();
            string query = "SELECT * FROM Empleados";

            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    empleados.Add(new Empleado
                    {
                        EmpleadoID = reader.GetInt32(reader.GetOrdinal("EmpleadoID")),
                        Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                        Apellido = reader.GetString(reader.GetOrdinal("Apellido")),
                        FechaNacimiento = reader.IsDBNull(reader.GetOrdinal("FechaNacimiento")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaNacimiento")),
                        FechaIngreso = reader.IsDBNull(reader.GetOrdinal("FechaIngreso")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaIngreso")),
                        Puesto = reader.GetString(reader.GetOrdinal("Puesto")),
                        SalarioBase = reader.GetDecimal(reader.GetOrdinal("SalarioBase")),
                        Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                    });
                }
            }
        }
        return empleados;
    }

    public async Task AgregarEmpleadoAsync(Empleado empleado)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();
            string query = "INSERT INTO Empleados (Nombre, Apellido, FechaNacimiento, FechaIngreso, Puesto, SalarioBase, Activo) VALUES (@Nombre, @Apellido, @FechaNacimiento, @FechaIngreso, @Puesto, @SalarioBase, @Activo)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                DateTime fechaMinima = new DateTime(1753, 1, 1);
                DateTime fechaNacimiento = empleado.FechaNacimiento < fechaMinima ? fechaMinima : empleado.FechaNacimiento;
                DateTime fechaIngreso = empleado.FechaIngreso < fechaMinima ? fechaMinima : empleado.FechaIngreso;

                command.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                command.Parameters.AddWithValue("@Apellido", empleado.Apellido);
                command.Parameters.AddWithValue("@FechaNacimiento", fechaNacimiento);
                command.Parameters.AddWithValue("@FechaIngreso", fechaIngreso);
                command.Parameters.AddWithValue("@Puesto", empleado.Puesto);
                command.Parameters.AddWithValue("@SalarioBase", empleado.SalarioBase);
                command.Parameters.AddWithValue("@Activo", empleado.Activo);

                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task EliminarEmpleadoAsync(int empleadoID)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();
            string query = "DELETE FROM Empleados WHERE EmpleadoID=@EmpleadoID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@EmpleadoID", empleadoID);
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}

using CURDmvc.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

public class EmpleadoDAO
{
    private string connectionString = "Server=DARLA\\SQLEXPRESS01;Database=EmpresaDB;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;";

    public List<Empleado> ObtenerEmpleados()
    {
        var empleados = new List<Empleado>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT * FROM Empleados";

            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    empleados.Add(new Empleado
                    {
                        EmpleadoID = (int)reader["EmpleadoID"],
                        Nombre = reader["Nombre"].ToString(),
                        Apellido = reader["Apellido"].ToString(),
                        FechaNacimiento = reader["FechaNacimiento"] != DBNull.Value ? (DateTime)reader["FechaNacimiento"] : DateTime.MinValue,
                        FechaIngreso = reader["FechaIngreso"] != DBNull.Value ? (DateTime)reader["FechaIngreso"] : DateTime.MinValue,
                        Puesto = reader["Puesto"].ToString(),
                        SalarioBase = (decimal)reader["SalarioBase"],
                        Activo = (bool)reader["Activo"]
                    });
                }
            }
        }
        return empleados;
    }

    public void AgregarEmpleado(Empleado empleado)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
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

                command.ExecuteNonQuery();
            }
        }
    }

    public void EliminarEmpleado(int empleadoID)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "DELETE FROM Empleados WHERE EmpleadoID=@EmpleadoID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@EmpleadoID", empleadoID);
                command.ExecuteNonQuery();
            }
        }
    }
}

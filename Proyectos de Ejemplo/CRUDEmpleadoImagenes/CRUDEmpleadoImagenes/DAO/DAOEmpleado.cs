using System;
using System.Collections.Generic;
using System.Configuration;

using System.IO;
using CRUDEmpleadoImagenes.Models;
using Microsoft.Data.SqlClient;


namespace CRUDEmpleadoImagenes.DAO
{
    public class DAOEmpleado
    {
        private readonly string connectionString;

        public DAOEmpleado(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("CnnEmpresaDB");
        }

        public void Insertar(Empleado empleado)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Empleado (Nombre, Cargo, Foto, Firma, DocumentoPDF) VALUES (@Nombre, @Cargo, @Foto, @Firma, @DocumentoPDF)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                cmd.Parameters.AddWithValue("@Cargo", empleado.Cargo);
                cmd.Parameters.AddWithValue("@Foto", (object)empleado.Foto ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Firma", (object)empleado.Firma ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DocumentoPDF", (object)empleado.DocumentoPDF ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Empleado> ObtenerTodos()
        {
            List<Empleado> empleados = new List<Empleado>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT EmpleadoId, Nombre, Cargo FROM Empleado";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    empleados.Add(new Empleado
                    {
                        EmpleadoId = Convert.ToInt32(reader["EmpleadoId"]),
                        Nombre = reader["Nombre"].ToString(),
                        Cargo = reader["Cargo"].ToString()
                    });
                }
            }

            return empleados;
        }

        public Empleado ObtenerPorId(int id)
        {
            Empleado empleado = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Empleado WHERE EmpleadoId = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    empleado = new Empleado
                    {
                        EmpleadoId = (int)reader["EmpleadoId"],
                        Nombre = reader["Nombre"].ToString(),
                        Cargo = reader["Cargo"].ToString(),
                        Foto = reader["Foto"] as byte[],
                        Firma = reader["Firma"] as byte[],
                        DocumentoPDF = reader["DocumentoPDF"] as byte[]
                    };
                }
            }

            return empleado;
        }

        public void Actualizar(Empleado empleado)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Empleado SET Nombre = @Nombre, Cargo = @Cargo, Foto = @Foto, Firma = @Firma, DocumentoPDF = @DocumentoPDF WHERE EmpleadoId = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                cmd.Parameters.AddWithValue("@Cargo", empleado.Cargo);
                cmd.Parameters.AddWithValue("@Foto", (object)empleado.Foto ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Firma", (object)empleado.Firma ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DocumentoPDF", (object)empleado.DocumentoPDF ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Id", empleado.EmpleadoId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Eliminar(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Empleado WHERE EmpleadoId = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public byte[] ObtenerArchivo(int id, string tipo)
        {
            byte[] archivo = null;
            string campo = tipo == "foto" ? "Foto" :
                           tipo == "firma" ? "Firma" :
                           tipo == "pdf" ? "DocumentoPDF" : null;

            if (campo == null)
                return null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = $"SELECT {campo} FROM Empleado WHERE EmpleadoId = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();

                archivo = cmd.ExecuteScalar() as byte[];
            }

            return archivo;
        }
    }
}

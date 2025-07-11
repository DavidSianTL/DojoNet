using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using CRUDPersonasWinForms.Models;

namespace CRUDPersonasWinForms.DAO
{
    public class PersonaDAO
    {
        private readonly string connectionString = "Server=localhost\\SQLEXPRESS;Database=EmpresaDB;Trusted_Connection=True;Integrated Security=True;TrustServerCertificate=True;";

        public List<Persona> ObtenerTodas()
        {
            List<Persona> lista = new List<Persona>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Persona";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Persona
                    {
                        Id = (int)reader["Id"],
                        Nombre = reader["Nombre"].ToString(),
                        Edad = (int)reader["Edad"],
                        Correo = reader["Correo"].ToString()
                    });
                }
            }

            return lista;
        }

        public void Agregar(Persona persona)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO Persona (Nombre, Edad, Correo) VALUES (@Nombre, @Edad, @Correo)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", persona.Nombre);
                cmd.Parameters.AddWithValue("@Edad", persona.Edad);
                cmd.Parameters.AddWithValue("@Correo", persona.Correo);
                cmd.ExecuteNonQuery();
            }
        }

        public void Actualizar(Persona persona)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE Persona SET Nombre=@Nombre, Edad=@Edad, Correo=@Correo WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", persona.Id);
                cmd.Parameters.AddWithValue("@Nombre", persona.Nombre);
                cmd.Parameters.AddWithValue("@Edad", persona.Edad);
                cmd.Parameters.AddWithValue("@Correo", persona.Correo);
                cmd.ExecuteNonQuery();
            }
        }

        public void Eliminar(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "DELETE FROM Persona WHERE Id=@Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}

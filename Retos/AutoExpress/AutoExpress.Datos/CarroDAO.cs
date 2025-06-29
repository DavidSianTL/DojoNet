using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AutoExpress.Entidades;

namespace AutoExpress.Datos
{
    public class CarroDAO
    {
        public List<Carro> ListarTodos()
        {
            List<Carro> carros = new List<Carro>();

            try
            {
                using (SqlConnection conexion = ConexionDB.ObtenerConexion())
                {
                    string query = "SELECT Id, Marca, Modelo, Año, Precio, Disponible FROM Carros ORDER BY Id";
                    SqlCommand comando = new SqlCommand(query, conexion);

                    conexion.Open();
                    SqlDataReader reader = comando.ExecuteReader();

                    while (reader.Read())
                    {
                        Carro carro = new Carro
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Marca = reader["Marca"].ToString(),
                            Modelo = reader["Modelo"].ToString(),
                            Año = Convert.ToInt32(reader["Año"]),
                            Precio = Convert.ToDecimal(reader["Precio"]),
                            Disponible = Convert.ToBoolean(reader["Disponible"])
                        };
                        carros.Add(carro);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar carros: " + ex.Message);
            }

            return carros;
        }

        public Carro ObtenerPorId(int id)
        {
            Carro carro = null;

            try
            {
                using (SqlConnection conexion = ConexionDB.ObtenerConexion())
                {
                    string query = "SELECT Id, Marca, Modelo, Año, Precio, Disponible FROM Carros WHERE Id = @Id";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@Id", id);

                    conexion.Open();
                    SqlDataReader reader = comando.ExecuteReader();

                    if (reader.Read())
                    {
                        carro = new Carro
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Marca = reader["Marca"].ToString(),
                            Modelo = reader["Modelo"].ToString(),
                            Año = Convert.ToInt32(reader["Año"]),
                            Precio = Convert.ToDecimal(reader["Precio"]),
                            Disponible = Convert.ToBoolean(reader["Disponible"])
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener carro: " + ex.Message);
            }

            return carro;
        }

        public int Agregar(Carro carro)
        {
            int nuevoId = 0;

            try
            {
                using (SqlConnection conexion = ConexionDB.ObtenerConexion())
                {
                    string query = @"INSERT INTO Carros (Marca, Modelo, Año, Precio, Disponible) 
                                   VALUES (@Marca, @Modelo, @Año, @Precio, @Disponible);
                                   SELECT SCOPE_IDENTITY();";

                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@Marca", carro.Marca);
                    comando.Parameters.AddWithValue("@Modelo", carro.Modelo);
                    comando.Parameters.AddWithValue("@Año", carro.Año);
                    comando.Parameters.AddWithValue("@Precio", carro.Precio);
                    comando.Parameters.AddWithValue("@Disponible", carro.Disponible);

                    conexion.Open();
                    nuevoId = Convert.ToInt32(comando.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar carro: " + ex.Message);
            }

            return nuevoId;
        }

        public bool Actualizar(Carro carro)
        {
            try
            {
                using (SqlConnection conexion = ConexionDB.ObtenerConexion())
                {
                    string query = @"UPDATE Carros SET 
                                   Marca = @Marca, 
                                   Modelo = @Modelo, 
                                   Año = @Año, 
                                   Precio = @Precio, 
                                   Disponible = @Disponible 
                                   WHERE Id = @Id";

                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@Id", carro.Id);
                    comando.Parameters.AddWithValue("@Marca", carro.Marca);
                    comando.Parameters.AddWithValue("@Modelo", carro.Modelo);
                    comando.Parameters.AddWithValue("@Año", carro.Año);
                    comando.Parameters.AddWithValue("@Precio", carro.Precio);
                    comando.Parameters.AddWithValue("@Disponible", carro.Disponible);

                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();

                    return filasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar carro: " + ex.Message);
            }
        }

        public bool Eliminar(int id)
        {
            try
            {
                using (SqlConnection conexion = ConexionDB.ObtenerConexion())
                {
                    string query = "DELETE FROM Carros WHERE Id = @Id";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@Id", id);

                    conexion.Open();
                    int filasAfectadas = comando.ExecuteNonQuery();

                    return filasAfectadas > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar carro: " + ex.Message);
            }
        }

        public bool Existe(int id)
        {
            try
            {
                using (SqlConnection conexion = ConexionDB.ObtenerConexion())
                {
                    string query = "SELECT COUNT(*) FROM Carros WHERE Id = @Id";
                    SqlCommand comando = new SqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@Id", id);

                    conexion.Open();
                    int count = Convert.ToInt32(comando.ExecuteScalar());

                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar existencia del carro: " + ex.Message);
            }
        }
    }
}

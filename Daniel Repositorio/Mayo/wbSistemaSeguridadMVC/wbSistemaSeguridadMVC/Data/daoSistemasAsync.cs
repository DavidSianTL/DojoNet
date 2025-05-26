using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using wbSistemaSeguridadMVC.Models;
using wbSistemaSeguridadMVC.Services;



namespace wbSistemaSeguridadMVC.Data
{
    public class daoSistemasAsync
    {
        private readonly string _connectionString;
     

        public daoSistemasAsync(string connectionString) {

            _connectionString = connectionString;
        }
       

        public async Task<List<Sistema>> ObtenerSistemasAsync()
        {

            var Sistemas = new List<Sistema>();

            string query = "SELECT id_sistema, nombre_sistema, descripcion FROM Sistemas";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Sistemas.Add(new Sistema
                            {
                                IdSistema = reader.GetInt32(0),
                                NombreSistema = reader.GetString(1),
                                Descripcion = reader.GetString(2)

                            });
                        }
                    }

                }

            }
            return Sistemas;
        }


        public async Task InsertarSistemaAsync(Sistema sistema)
        {
            string sql = "INSERT INTO Sistemas (nombre_sistema, descripcion) VALUES (@nombre_sistema, @descripcion)";

            using (SqlConnection cnn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    cmd.Parameters.AddWithValue("@nombre_sistema", sistema.NombreSistema);
                    cmd.Parameters.AddWithValue("@descripcion", sistema.Descripcion);
                    await cnn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                }


            }
        }

        public async Task<Sistema> ObtenerSistemaPorIdAsync(int Id) { 
        
            Sistema sistema = null;

            string sql = "SELECT id_sistema, nombre_sistema, descripcion FROM Sistemas WHERE id_sistema = @Id";

            using (SqlConnection cnn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, cnn))
                {
                    cmd.Parameters.AddWithValue("@Id", Id);
                    await cnn.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync()) {
                            sistema = new Sistema
                            {
                                IdSistema = reader.GetInt32(0),
                                NombreSistema = reader.GetString(1),
                                Descripcion = reader.GetString(2)

                            };
                       
                        }
                    }
                }
            }
            return sistema;
        }


        public async Task ActualizarSistemaAsync(Sistema sistema) 
        {
            string sql = "UPDATE Sistemas SET nombre_sistema = @NombreSistema, descripcion = @Descripcion WHERE id_sistema = @Id";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@NombreSistema", sistema.NombreSistema);
                    cmd.Parameters.AddWithValue("@Descripcion", sistema.Descripcion);
                    cmd.Parameters.AddWithValue("@Id", sistema.IdSistema);

                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();   
                   

                }
            }
        
        
        
        }

        public async Task EliminarsistemaAsync(int id)
        {
            string sql = "DELETE FROM sistemas WHERE id_sistema = @Id";


            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);


                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync(); // Ejecuta el update
                }

            }
        }

    }
}

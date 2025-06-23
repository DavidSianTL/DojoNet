using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Models.DepartamentosEmpresa;
using ProyectoDojoGeko.Models.Usuario;

namespace ProyectoDojoGeko.Data
{
    public class daoDepartamentosEmpresaWSAsync
    {
        private readonly string _connectionString;

        public daoDepartamentosEmpresaWSAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener la lista de departamentos empresa
        public async Task<List<DepartamentoEmpresaViewModel>> ObtenerDepartamentosEmpresa()
        {

            // Declaración de la lista de departamentos empresa
            var departamentosEmpresa = new List<DepartamentoEmpresaViewModel>();

            // Nombre del procedimiento almacenado que se va a ejecutar
            string query = @"SELECT de.IdDepartamentoEmpresa, de.FK_IdDepartamento, de.FK_IdEmpresa, d.Nombre AS NombreDepartamento, e.Nombre AS NombreEmpresa
                                          FROM DepartamentosEmpresa de
                                          JOIN Departamentos d ON de.FK_IdDepartamento = d.IdDepartamento
                                          JOIN Empresas e ON de.FK_IdEmpresa = e.IdEmpresa";

            // Conexión a la base de datos y ejecución del procedimiento almacenado
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {

                // Abre la conexión de forma asincrona
                await conn.OpenAsync();

                // Crea el comando para ejecutar el procedimiento almacenado
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    // Establece el tipo de comando como procedimiento almacenado
                    cmd.CommandType = CommandType.Text;

                    // Ejecuta el comando y obtiene un lector de datos as�ncrono
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {

                        // Mientras haya registros, los lee y los agrega a la lista de departamentos empresa
                        while (await reader.ReadAsync())
                        {

                            // Crea un nuevo objeto DepartamentoEmpresaViewModel y lo llena con los datos del lector
                            departamentosEmpresa.Add(new DepartamentoEmpresaViewModel
                            {
                                // Asigna los valores de las columnas del lector a las propiedades del modelo
                                IdDepartamentoEmpresa = reader.GetInt32(reader.GetOrdinal("IdDepartamentoEmpresa")),
                                FK_IdDepartamento = reader.GetInt32(reader.GetOrdinal("FK_IdDepartamento")),
                                FK_IdEmpresa = reader.GetInt32(reader.GetOrdinal("FK_IdEmpresa")),
                                NombreDepartamento = reader.GetString(reader.GetOrdinal("NombreDepartamento")),
                                NombreEmpresa = reader.GetString(reader.GetOrdinal("NombreEmpresa"))
                            });
                        }
                    }
                }
            }

            // Devuelve la lista de departamentos empresa obtenida
            return departamentosEmpresa;
        }


        public DepartamentoEmpresaViewModel? ObtenerPorId(int idDepartamentoEmpresa)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM DepartamentosEmpresa WHERE IdDepartamentoEmpresa = @id", conn);
                cmd.Parameters.AddWithValue("@IdDepartamentoEmpresa", idDepartamentoEmpresa);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new DepartamentoEmpresaViewModel
                        {
                            IdDepartamentoEmpresa = Convert.ToInt32(reader["IdDepartamentoEmpresa"]),
                            FK_IdDepartamento = Convert.ToInt32(reader["FK_IdDepartamento"]),
                            FK_IdEmpresa = Convert.ToInt32(reader["FK_IdEmpresa"])
                        };
                    }
                }
            }
            return null;
        }

        public async Task<List<DepartamentoEmpresaViewModel>> ObtenerDepartamentosPorEmpresaAsync(int idEmpresa)
        {
            var departamentosEmpresa = new List<DepartamentoEmpresaViewModel>();

            string query = @"SELECT de.IdDepartamentoEmpresa, de.FK_IdDepartamento, de.FK_IdEmpresa, 
                            d.Nombre AS NombreDepartamento, e.Nombre AS NombreEmpresa
                           FROM DepartamentosEmpresa de
                           JOIN Departamentos d ON de.FK_IdDepartamento = d.IdDepartamento
                           JOIN Empresas e ON de.FK_IdEmpresa = e.IdEmpresa
                           WHERE de.FK_IdEmpresa = @IdEmpresa";

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdEmpresa", idEmpresa);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            departamentosEmpresa.Add(new DepartamentoEmpresaViewModel
                            {
                                IdDepartamentoEmpresa = reader.GetInt32(reader.GetOrdinal("IdDepartamentoEmpresa")),
                                FK_IdDepartamento = reader.GetInt32(reader.GetOrdinal("FK_IdDepartamento")),
                                FK_IdEmpresa = reader.GetInt32(reader.GetOrdinal("FK_IdEmpresa")),
                                NombreDepartamento = reader.GetString(reader.GetOrdinal("NombreDepartamento")),
                                NombreEmpresa = reader.GetString(reader.GetOrdinal("NombreEmpresa"))
                            });
                        }
                    }
                }
            }


            return departamentosEmpresa;
        }

        public async Task<bool> InsertarDepartamentoEmpresaAsync(DepartamentoEmpresaViewModel model)
        {
            try
            {
                string procedure = "sp_InsertarDepartamentoEmpresa";
                using SqlConnection cnn = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand(procedure, cnn)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@FK_IdDepartamento", model.FK_IdDepartamento);
                cmd.Parameters.AddWithValue("@FK_IdEmpresa", model.FK_IdEmpresa);
                await cnn.OpenAsync();
                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                // Verifica si se insertó al menos un registro
                return rowsAffected > 0; // Retorna true si se insertó al menos un registro
            }
            catch (Exception ex)
            {
                throw new Exception("Error al insertar el departamento y empresa", ex);
            }
        }


        public async Task<bool> ActualizarDepartamentoEmpresaAsync(DepartamentoEmpresaViewModel departamentoEmpresa)
        {
            try
            {
                string procedure = "sp_ActualizarDepartamentoEmpresa";
                using SqlConnection cnn = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand(procedure, cnn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@IdDepartamentoEmpresa", departamentoEmpresa.IdDepartamentoEmpresa);
                cmd.Parameters.AddWithValue("@FK_IdDepartamento", departamentoEmpresa.FK_IdDepartamento);
                cmd.Parameters.AddWithValue("@FK_IdEmpresa", departamentoEmpresa.FK_IdEmpresa);
                await cnn.OpenAsync();
                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                // Verifica si se actualizó al menos un registro
                return rowsAffected > 0; // Retorna true si se actualizó al menos un registro
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el departamento y empresa", ex);
            }
        }


        public async Task<bool> EliminarDepartamentoEmpresaAsync(int idDepartamentoEmpresa)
        {
            try
            {
                string procedure = "sp_EliminarDepartamentoEmpresa";

                using SqlConnection cnn = new SqlConnection(_connectionString);

                using SqlCommand cmd = new SqlCommand(procedure, cnn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@IdDepartamentoEmpresa", idDepartamentoEmpresa);

                await cnn.OpenAsync();

                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                // Verifica si se eliminó al menos un registro
                return rowsAffected > 0; // Retorna true si se eliminó al menos un registro

            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el departamento y empresa", ex);
            }
        }

    }
}

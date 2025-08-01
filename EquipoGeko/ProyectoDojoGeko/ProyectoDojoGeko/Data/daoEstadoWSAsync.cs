using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoDojoGeko.Data
{
    public class daoEstadoWSAsync
    {
        // Variable global para la conexión
        private readonly string _connectionString;

        // Constructor para inicializar la cadena de conexión
        public daoEstadoWSAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Método para obtener la lista de estados
        public async Task<List<EstadosViewModel>> ObtenerEstadosAsync()
        {
            var estados = new List<EstadosViewModel>();
            string procedure = "sp_ListarEstados";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            estados.Add(new EstadosViewModel
                            {
                                IdEstado = reader.GetInt32(reader.GetOrdinal("IdEstado")),
                                Estado = reader.GetString(reader.GetOrdinal("Estado")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                            });
                        }
                    }
                }
            }
            return estados;
        }

        // Método para buscar un estado por su ID
        public async Task<EstadosViewModel> ObtenerEstadoPorIdAsync(int id)
        {
            string procedure = "sp_ListarEstadoId";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdEstado", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new EstadosViewModel
                            {
                                IdEstado = reader.GetInt32(reader.GetOrdinal("IdEstado")),
                                Estado = reader.GetString(reader.GetOrdinal("Estado")),
                                Descripcion = reader.GetString(reader.GetOrdinal("Descripcion")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("Activo"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Método para insertar un nuevo estado
        public async Task<int> InsertarEstadoAsync(EstadosViewModel estado)
        {
            string procedure = "sp_InsertarEstado";
            int idGenerado = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Estado", estado.Estado);
                    cmd.Parameters.AddWithValue("@Descripcion", estado.Descripcion);
                    cmd.Parameters.AddWithValue("@Activo", estado.Activo);

                    // Parámetro de salida para el ID generado
                    SqlParameter idParam = new SqlParameter("@IdEstado", SqlDbType.Int);
                    idParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(idParam);

                    await cmd.ExecuteNonQueryAsync();
                    idGenerado = (int)idParam.Value;
                }
            }
            return idGenerado;
        }

        // Método para actualizar un estado existente
        public async Task<bool> ActualizarEstadoAsync(EstadosViewModel estado)
        {
            string procedure = "sp_ActualizarEstado";
            int filasAfectadas = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdEstado", estado.IdEstado);
                    cmd.Parameters.AddWithValue("@Estado", estado.Estado);
                    cmd.Parameters.AddWithValue("@Descripcion", estado.Descripcion);
                    cmd.Parameters.AddWithValue("@Activo", estado.Activo);

                    filasAfectadas = await cmd.ExecuteNonQueryAsync();
                }
            }
            return filasAfectadas > 0;
        }

        // Método para eliminar un estado (eliminación lógica)
        public async Task<bool> EliminarEstadoAsync(int id)
        {
            string procedure = "sp_EliminarEstado";
            int filasAfectadas = 0;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdEstado", id);

                    filasAfectadas = await cmd.ExecuteNonQueryAsync();
                }
            }
            return filasAfectadas > 0;
        }
    }
}
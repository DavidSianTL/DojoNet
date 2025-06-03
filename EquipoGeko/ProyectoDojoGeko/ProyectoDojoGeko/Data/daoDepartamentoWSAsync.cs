using System.Data;
using Microsoft.Data.SqlClient;
using ProyectoDojoGeko.Models;

namespace ProyectoDojoGeko.Data
{
    public class daoDepartamentoWSAsync
    {
        private readonly string _connectionString;

        public daoDepartamentoWSAsync(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<DepartamentoViewModel>> ObtenerDepartamentosAsync()
        {
            var departamentos = new List<DepartamentoViewModel>();
            string procedure = "sp_ListarDepartamentos";

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
                            departamentos.Add(new DepartamentoViewModel
                            {
                                IdDepartamento = reader.GetInt32(reader.GetOrdinal("IdDepartamento")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? "" : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Codigo = reader.GetString(reader.GetOrdinal("Codigo")),
                                Estado = reader.GetBoolean(reader.GetOrdinal("Estado")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion"))
                            });
                        }
                    }
                }
            }

            return departamentos;
        }

        public async Task<DepartamentoViewModel> ObtenerDepartamentoPorIdAsync(int Id)
        {
            string procedure = "sp_ListarDepartamentoId";

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand(procedure, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdDepartamento", Id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new DepartamentoViewModel
                            {
                                IdDepartamento = reader.GetInt32(reader.GetOrdinal("IdDepartamento")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? "" : reader.GetString(reader.GetOrdinal("Descripcion")),
                                Codigo = reader.GetString(reader.GetOrdinal("Codigo")),
                                Estado = reader.GetBoolean(reader.GetOrdinal("Estado")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("FechaCreacion"))
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<int> InsertarDepartamentoAsync(DepartamentoViewModel departamento)
        {
            var parametros = new[]
            {
                new SqlParameter("@Nombre", departamento.Nombre),
                new SqlParameter("@Descripcion", departamento.Descripcion),
                new SqlParameter("@Codigo", departamento.Codigo)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_InsertarDepartamento", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> ActualizarDepartamentoAsync(DepartamentoViewModel departamento)
        {
            var parametros = new[]
            {
                new SqlParameter("@IdDepartamento", departamento.IdDepartamento),
                new SqlParameter("@Nombre", departamento.Nombre),
                new SqlParameter("@Descripcion", departamento.Descripcion),
                new SqlParameter("@Codigo", departamento.Codigo),
                new SqlParameter("@Estado", departamento.Estado)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_ActualizarDepartamento", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<int> EliminarDepartamentoAsync(int Id)
        {
            var parametros = new[]
            {
                new SqlParameter("@IdDepartamento", Id)
            };

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (SqlCommand cmd = new SqlCommand("sp_EliminarDepartamento", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parametros);
                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
